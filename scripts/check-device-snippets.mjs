// Collects the app code written into the Device SDK's device-side pages (src/content/docs/device-sdk, not server/)
// into one small project per language under examples/device/, so CI compiles or type-checks every snippet against
// the published SDKs:
//
//   kotlin, java   examples/device/android   Gradle, net.ohrtech:device-sdk
//   dart           examples/device/flutter   flutter analyze, ohrtech_device
//   ts, tsx        examples/device/rn        tsc, @ohrtech/device-sdk-react-native
//   csharp         examples/device/dotnet    dotnet build, OhrTech.Device
//
// A fragment (statements) is wrapped in a function that has the names a page's text supplies (activity, context, ...;
// see each project's prelude). A snippet that declares classes or top-level functions is a file of its own. A snippet
// not meant to compile gets the fence meta `nocheck`; server C# on these pages, `server` (scripts/check-snippets.mjs).
//
//   node scripts/check-device-snippets.mjs && scripts/check-device-snippets.sh
import { mkdir, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import { join, relative } from 'node:path';

const root = new URL('..', import.meta.url).pathname;
const pages = join(root, 'src/content/docs/device-sdk');
const out = join(root, 'examples/device');

async function* files(dir) {
	for (const entry of await readdir(dir, { withFileTypes: true })) {
		const path = join(dir, entry.name);
		if (entry.isDirectory()) {
			if (entry.name !== 'server') yield* files(path);
		} else if (/\.mdx?$/.test(entry.name)) yield path;
	}
}

const langs = { kotlin: 'kotlin', kt: 'kotlin', java: 'java', dart: 'dart', ts: 'ts', tsx: 'ts', typescript: 'ts', csharp: 'cs', cs: 'cs' };
const snippets = { kotlin: [], java: [], dart: [], ts: [], cs: [] };
for await (const file of files(pages)) {
	const text = await readFile(file, 'utf8');
	for (const m of text.matchAll(/^([ \t]*)```(\w+)([^\n]*)\n([\s\S]*?)^\1```/gm)) {
		const lang = langs[m[2]];
		if (!lang || /\b(nocheck|server)\b/.test(m[3])) continue;
		const indent = m[1];
		const code = m[4].split('\n').map((l) => (l.startsWith(indent) ? l.slice(indent.length) : l)).join('\n').trimEnd();
		snippets[lang].push({ where: `${relative(root, file)}:${text.slice(0, m.index).split('\n').length}`, code });
	}
}

const header = (where, c = '//') => `${c} ${where}: written by scripts/check-device-snippets.mjs. Do not edit.\n`;
const split = (code, importRe) => {
	const imports = [], body = [];
	for (const l of code.split('\n')) (importRe.test(l) ? imports : body).push(l);
	return { imports, body: body.join('\n') };
};
const indent = (s, n) => s.replace(/^(?=.)/gm, ' '.repeat(n));

async function fresh(dir) {
	await rm(dir, { recursive: true, force: true });
	await mkdir(dir, { recursive: true });
}

// Kotlin: each snippet a file in its own package. Fragments run as an extension of Prelude (its names in scope).
const ktDir = join(out, 'android/src/main/java/snippets/generated/kt');
await fresh(ktDir);
const ktImports = ['android.app.Activity', 'android.content.Context', 'kotlinx.coroutines.*', 'kotlinx.coroutines.flow.*',
	'net.ohrtech.device.*', 'net.ohrtech.device.keys.*', 'net.ohrtech.device.push.*', 'snippets.Prelude'];
for (const [i, { where, code }] of snippets.kotlin.entries()) {
	const { imports, body } = split(code, /^import [\w.*]+(\s+as\s+\w+)?\s*$/);
	// A class, object or function at the left margin: the snippet is a file of its own.
	const declares = /^(class|object|interface|enum class|abstract class|open class|data class|sealed class|fun|suspend fun|private fun|internal fun|@\w+)\b/m.test(body);
	const file = declares
		? body
		: `@Suppress("UNUSED_VARIABLE", "UNUSED_PARAMETER", "UNREACHABLE_CODE", "unused")\nsuspend fun Prelude.snippet() {\n${indent(body, 4)}\n}`;
	const all = [...new Set([...ktImports.map((x) => `import ${x}`), ...imports.map((l) => l.trim())])];
	await writeFile(join(ktDir, `S${i}.kt`), `${header(where)}@file:Suppress("unused")\npackage snippets.generated.kt.s${i}\n\n${all.join('\n')}\n\n${file}\n`);
}

// Java: each snippet a file in its own package; fragments are the body of run() in a subclass of JPrelude.
const javaDir = join(out, 'android/src/main/java/snippets/generated/java');
await fresh(javaDir);
const javaImports = ['android.app.Activity', 'android.content.Context', 'java.io.Closeable', 'java.util.*', 'java.util.concurrent.*',
	'net.ohrtech.device.*', 'net.ohrtech.device.keys.*', 'net.ohrtech.device.push.*', 'snippets.JPrelude'];
for (const [i, { where, code }] of snippets.java.entries()) {
	const { imports, body } = split(code, /^import (static )?[\w.*]+;\s*$/);
	const declares = /^(public |final |abstract )*(class|interface|enum|record) \w+/m.test(body);
	const file = declares ? body : `class Snippet extends JPrelude {\n    void run() throws Exception {\n${indent(body, 8)}\n    }\n}`;
	const all = [...new Set([...javaImports.map((x) => `import ${x};`), ...imports.map((l) => l.trim())])];
	await mkdir(join(javaDir, `s${i}`), { recursive: true });
	// A public class (an Android component must be one) names its file.
	const publicType = declares ? body.match(/^public (?:final |abstract )*(?:class|interface|enum|record) (\w+)/m)?.[1] : null;
	await writeFile(join(javaDir, `s${i}`, `${publicType ?? 'Snippet'}.java`), `${header(where)}package snippets.generated.java.s${i};\n\n${all.join('\n')}\n\n${file}\n`);
}

// Dart: each snippet a library; fragments are the body of an async function with the prelude's names.
const dartDir = join(out, 'flutter/lib/generated');
await fresh(dartDir);
for (const [i, { where, code }] of snippets.dart.entries()) {
	const { imports, body } = split(code, /^import '[^']+'( as \w+)?( (show|hide) [\w, ]+)?;\s*$/);
	const declares = /^(class |abstract |enum |typedef |mixin |extension |@pragma|(?:Future<[^\n]*>|Stream<[^\n]*>|void|Widget|int|String|bool)\s+\w+\s*[(<])/m.test(body);
	const file = declares ? body : `Future<void> snippet() async {\n${indent(body, 2)}\n}`;
	const all = [...new Set(["import 'dart:async';", "import 'dart:convert';", "import 'dart:typed_data';",
		"import 'package:flutter/material.dart';", "import 'package:ohrtech_device/ohrtech_device.dart';",
		"import '../prelude.dart';", ...imports.map((l) => l.trim())])];
	await writeFile(join(dartDir, `s${i}.dart`), `${header(where)}// ignore_for_file: type=lint, unused_import, unused_local_variable, unused_element, dead_code\n${all.join('\n')}\n\n${file}\n`);
}

// TypeScript: each snippet a module; everything but its imports runs in an async function. The SDK's exports and
// the prelude's names are in scope when the snippet doesn't import them itself.
const tsDir = join(out, 'rn/src/generated');
await fresh(tsDir);
const rnTypes = process.env.OHR_RN_PATH
	? join(process.env.OHR_RN_PATH, 'lib/typescript/src')
	: join(out, 'rn/node_modules/@ohrtech/device-sdk-react-native/lib/typescript/src');
const exportsOf = async (dir) => {
	const names = new Set();
	for (const f of await readdir(dir)) {
		if (!f.endsWith('.d.ts')) continue;
		const t = await readFile(join(dir, f), 'utf8');
		for (const m of t.matchAll(/^export (?:declare )?(?:abstract )?(?:const|function|class|let)\s+(\w+)/gm)) names.add(m[1]);
	}
	return names;
};
const sdkNames = await exportsOf(rnTypes).catch(() => new Set());
if (snippets.ts.length && sdkNames.size === 0) console.warn('check-device-snippets: no @ohrtech/device-sdk-react-native types found; set OHR_RN_PATH or npm ci in examples/device/rn');
const basePrelude = ['activity', 'serverNonce', 'ciphertext', 'enc', 'ED25519_PUBLIC_KEY_BASE64'];
const preludeNames = new Set([...(await readFile(join(out, 'rn/src/prelude.ts'), 'utf8')).matchAll(/^export (?:const|function|async function|class|let)\s+(\w+)/gm)].map((m) => m[1]));
for (const [i, { where, code }] of snippets.ts.entries()) {
	const { imports, body } = split(code, /^import .* from '[^']+';\s*$|^import '[^']+';\s*$/);
	const imported = new Set(imports.flatMap((l) => (l.match(/\{([^}]*)\}/)?.[1] ?? '').split(',').map((x) => x.trim().split(/\s+as\s+/).pop()).filter(Boolean)));
	const used = [...sdkNames].filter((n) => !imported.has(n) && new RegExp(`\\b${n}\\b`).test(body));
	const all = [...imports.map((l) => l.trim())];
	if (used.length) all.push(`import { ${used.join(', ')} } from '@ohrtech/device-sdk-react-native';`);
	// The prelude's other names, when the snippet uses them.
	const more = [...preludeNames].filter((n) => !basePrelude.includes(n) && !imported.has(n) && new RegExp(`\\b${n}\\b`).test(body));
	all.push(`import { ${[...basePrelude, ...more].join(', ')} } from '../prelude';`);
	await writeFile(join(tsDir, `s${i}.tsx`), `${header(where)}/* eslint-disable */\n${all.join('\n')}\n\nexport async function snippet() {\n${indent(body, 2)}\n}\nvoid activity; void serverNonce; void ciphertext; void enc; void ED25519_PUBLIC_KEY_BASE64;\n`);
}

// C#: one file; fragments are static methods with the prelude's names as locals, declarations go after them.
const csFns = [], csTypes = [], csUsings = new Set(['using OhrTech.Device;', 'using System.Text.Json.Nodes;']);
const csPrelude = {
	activity: 'Android.App.Activity activity = null!;',
	context: 'Android.Content.Context context = null!;',
	serverNonce: 'var serverNonce = new byte[32];',
	challenge: 'var challenge = new byte[32];',
	ciphertext: 'var ciphertext = new byte[1120];',
	enc: 'var enc = new byte[32];',
	cancellationToken: 'var cancellationToken = CancellationToken.None;',
	ed25519PublicKey: 'var ed25519PublicKey = "";',
	mlDsa65PublicKey: 'var mlDsa65PublicKey = "";',
	theirPublicKey: 'var theirPublicKey = new byte[1184];',
	SendToServer: 'static Task SendToServer(params object?[] values) => Task.CompletedTask;',
	label: 'Android.Widget.TextView label = null!;',
	wifiView: 'OhrTech.Device.OhrWifiView wifiView = null!;',
	staffPassword: 'var staffPassword = "";',
	myServer: 'var myServer = (NewChallengeAsync: (Func<Task<string>>)(() => Task.FromResult("")), RegisterAsync: (Func<string, Task>)(_ => Task.CompletedTask));',
};
for (const [i, { where, code }] of snippets.cs.entries()) {
	const { imports, body } = split(code, /^using [\w.]+;\s*$/);
	imports.forEach((l) => csUsings.add(l.trim()));
	if (/^(public |internal |sealed |static |partial |abstract )*(class|record|interface|enum) \w+/m.test(body)) {
		csTypes.push(`// ${where}\n${body}`);
		continue;
	}
	// A type before the name declares it; a keyword before it (await SendToServer(…)) doesn't.
	const declared = (name) => [...body.matchAll(new RegExp(`([\\w>\\]?]+)\\s+${name}\\s*[=;(]`, 'g'))]
		.some((m) => !/^(await|return|throw|new|in|is|as|yield|else|case)$/.test(m[1]));
	const needed = Object.entries(csPrelude).filter(([n]) => new RegExp(`\\b${n}\\b`).test(body) && !declared(n)).map(([, l]) => l);
	csFns.push(`    // ${where}\n    static async Task Snippet${i}()\n    {\n${indent([...needed, body].join('\n'), 8)}\n    }`);
}
await mkdir(join(out, 'dotnet'), { recursive: true });
await writeFile(join(out, 'dotnet/Snippets.g.cs'), `${header('every device C# snippet')}#pragma warning disable CS8321, CS0168, CS0219, CS1998, CS0162, CS4014\n` +
	`${[...csUsings].join('\n')}\n\nnamespace Snippets;\n\ninternal static class All\n{\n${csFns.join('\n\n')}\n}\n\n${csTypes.join('\n\n')}\n`);

console.log(`check-device-snippets: ${Object.entries(snippets).map(([l, s]) => `${s.length} ${l}`).join(', ')}`);
