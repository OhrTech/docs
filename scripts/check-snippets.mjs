// Collects every C# snippet written into the pages (```csharp blocks, and the home page's hero) into
// examples/.page-snippets.cs, one function each, so CI compiles them against the published SDK
// like the complete examples. A snippet that is not meant to compile on its own gets the fence
// meta `nocheck` (```csharp nocheck).
//
//   node scripts/check-snippets.mjs && dotnet build examples/.page-snippets.cs
import { readFile, writeFile, readdir } from 'node:fs/promises';
import { join, relative } from 'node:path';

const root = new URL('..', import.meta.url).pathname;
const sdk = process.env.OHR_SDK_PROJECT
	? `#:project ${process.env.OHR_SDK_PROJECT}`
	: `#:package OhrTech.Management@${process.env.OHR_SDK_VERSION ?? '1.0.1'}`;

async function* files(dir) {
	for (const entry of await readdir(dir, { withFileTypes: true })) {
		const path = join(dir, entry.name);
		if (entry.isDirectory()) yield* files(path);
		else if (/\.mdx?$/.test(entry.name)) yield path;
	}
}

const snippets = [];
for await (const file of files(join(root, 'src/content/docs'))) {
	const text = await readFile(file, 'utf8');
	for (const m of text.matchAll(/^([ \t]*)```(?:csharp|cs)(?!\w)([^\n]*)\n([\s\S]*?)^\1```/gm)) {
		if (/\bnocheck\b/.test(m[2])) continue;
		const indent = m[1];
		const code = m[3].split('\n').map((l) => (l.startsWith(indent) ? l.slice(indent.length) : l)).join('\n');
		snippets.push({ where: `${relative(root, file)}:${text.slice(0, m.index).split('\n').length}`, code });
	}
}
const hero = (await readFile(join(root, 'src/components/Hero.astro'), 'utf8')).match(/const sample = `([\s\S]*?)`;/);
if (hero) snippets.push({ where: 'src/components/Hero.astro', code: hero[1].replace(/\\\$/g, '$') });

// Names the snippets use without declaring, as a page's surrounding text would.
const prelude = {
	api: 'using var api = OhrManagementClient.ForServiceAccount("ohrs_");',
	token: 'var token = "ohrs_";',
	patch: 'var patch = new PolicyPatchDto();',
	deviceId: 'long deviceId = 42;',
};

const functions = snippets.map(({ where, code }, i) => {
	const body = code
		.split('\n')
		.filter((l) => !/^using [\w.]+;\s*$/.test(l) && !l.startsWith('#:'))
		.join('\n')
		.replace(/<iframe[^\n]*$/gm, '');
	const declared = (name) => new RegExp(`\\b(var|using var|long|string)\\s+${name}\\b`).test(body);
	const needed = Object.entries(prelude)
		.filter(([name]) => new RegExp(`\\b${name}\\b`).test(body) && !declared(name))
		.map(([, line]) => line);
	return `// ${where}\nstatic async Task Snippet${i}()\n{\n${[...needed, body].join('\n').replace(/^/gm, '    ')}\n}`;
});

await writeFile(
	join(root, 'examples/.page-snippets.cs'),
	`${sdk}\n// Written by scripts/check-snippets.mjs from the pages' C# snippets. Do not edit.\n` +
		`#pragma warning disable CS8321, CS0168, CS8625, CS0219, CS1998\nusing OhrTech.Management;\n\n` +
		`await Task.CompletedTask;\n\n${functions.join('\n\n')}\n`,
);
console.log(`check-snippets: ${snippets.length} snippets written to examples/.page-snippets.cs`);
