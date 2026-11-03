// Copies the Management API's OpenAPI document into the site (public/management-api/openapi.json)
// and adds a C# SDK example to every operation (x-codeSamples), next to the generated curl, Python,
// JavaScript and other examples the reference shows.
//
//   npm run sync-openapi                       from github.com/OhrTech/management-api (main)
//   npm run sync-openapi -- ../management-api/openapi.json    from a local file
//
// The result is committed, so a build never depends on the network. An operation without a sample
// here fails the sync: new operations get one when they are added.
import { readFile, writeFile, mkdir } from 'node:fs/promises';

const source = process.argv[2] ?? 'https://raw.githubusercontent.com/OhrTech/management-api/main/openapi.json';
const out = new URL('../public/management-api/openapi.json', import.meta.url);

const text = source.startsWith('http') ? await (await fetch(source)).text() : await readFile(source, 'utf8');
const doc = JSON.parse(text);

const client = 'using var api = OhrManagementClient.ForServiceAccount(token);\n\n';
const sessionId = 'var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");\n';

// One C# call per operationId, in the SDK's terms (OhrTech.Management on NuGet).
const samples = {
	GetMe: 'var me = await api.GetMeAsync();',
	ListDevices: 'var devices = await api.ListDevicesAsync(online: true);',
	GetDevice: 'var device = await api.GetDeviceAsync(42);',
	MoveDevice: 'await api.MoveDeviceAsync(42, new MoveDeviceRequest { FleetId = 7 });',
	ListEnrolments: 'var enrolments = await api.ListEnrolmentsAsync(42);',
	ListInstalledApps: 'var apps = await api.ListInstalledAppsAsync(42);',
	RequestCheckIn: 'var checkIn = await api.RequestCheckInAsync(42);',
	ListFleets: 'var fleets = await api.ListFleetsAsync();',
	ListAdbSessions: 'var sessions = await api.ListAdbSessionsAsync(42);',
	OpenAdbSession:
		'// The ohr command line tool opens ADB sessions and runs the encrypted tunnel:\n//   ohr adb connect 42\n' +
		'var session = await api.OpenAdbSessionAsync(42, new OpenAdbSessionRequest\n{\n    AdbPublicKey = adbPublicKey,\n    ClientKey = clientKey,\n});',
	GetAdbSession: sessionId + 'var session = await api.GetAdbSessionAsync(sessionId);',
	CloseAdbSession: sessionId + 'await api.CloseAdbSessionAsync(sessionId);',
	ListRemoteControlSessions: 'var sessions = await api.ListRemoteControlSessionsAsync(42);',
	OpenRemoteControlSession:
		'var rc = await api.OpenRemoteControlSessionAsync(42, new OpenRemoteControlRequest\n{\n    EmbedOrigins = ["https://support.example.com"],\n});\n// <iframe src="{rc.Embed.Url}" allow="fullscreen; clipboard-read"></iframe>',
	GetRemoteControlSession: sessionId + 'var session = await api.GetRemoteControlSessionAsync(sessionId);',
	CloseRemoteControlSession: sessionId + 'await api.CloseRemoteControlSessionAsync(sessionId);',
	JoinRemoteControlSession: sessionId + 'var rc = await api.JoinRemoteControlSessionAsync(sessionId);',
	GetSettingsCatalog: 'var catalog = await api.GetSettingsCatalogAsync();',
	GetDeviceSettings: 'var settings = await api.GetDeviceSettingsAsync(42);',
	ReplaceDeviceSettings:
		'var settings = await api.ReplaceDeviceSettingsAsync(42, new PolicyDto\n{\n    Settings = new()\n    {\n        ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },\n    }\n});',
	UpdateDeviceSettings:
		'var settings = await api.UpdateDeviceSettingsAsync(42, new PolicyPatchDto\n{\n    Settings = new()\n    {\n        ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },\n        ["dev.usb_debugging"] = null, // remove the override\n    }\n});',
	PushDeviceSettings:
		'await api.PushDeviceSettingsAsync(42, new PushSettingsRequest { Reapply = ["display.brightness"] });',
	StartDeviceLogin: null, // the SDK runs the whole device code flow:
	PollDeviceLogin: null,
	RefreshLogin: null,
	ListTasks: 'var tasks = await api.ListTasksAsync(42, limit: 50);',
	CreateTask:
		'var task = await api.CreateTaskAsync(42, new CreateTaskRequest\n{\n    TaskClass = "ShowToast",\n    Parameters = ["Please plug me in"],\n});',
	GetTask: 'var task = await api.GetTaskAsync(42, 9131);',
	ShowMessage: 'var task = await api.ShowMessageAsync(42, new ShowMessageRequest { Text = "Please plug me in" });',
};

const signIn =
	'// The SDK runs the device code flow and refreshes the access token for you.\n' +
	'var session = await OhrUserSession.SignInAsync(p =>\n    Console.WriteLine($"Open {p.VerificationUri} and enter {p.UserCode}"));\n' +
	'using var api = OhrManagementClient.ForUser(session);';

// Short operation names for the reference's sidebar; the API's one-sentence summaries become the
// description when an operation has none.
const titles = {
	GetMe: 'Who am I',
	RequestCheckIn: 'Request a check-in',
	StartDeviceLogin: 'Start a sign-in',
	PollDeviceLogin: 'Finish a sign-in',
	RefreshLogin: 'Refresh tokens',
	ShowMessage: 'Show a message',
	GetSettingsCatalog: 'Settings catalog',
	PushDeviceSettings: 'Push settings again',
};
const title = (id) =>
	titles[id] ?? id.replace(/(?!^)([A-Z])/g, ' $1').replace(/ (\w)/g, (m) => m.toLowerCase()).replace(/\badb\b/, 'ADB');

const missing = [];
for (const ops of Object.values(doc.paths)) {
	for (const op of Object.values(ops)) {
		if (!(op.operationId in samples)) {
			missing.push(op.operationId);
			continue;
		}
		if (op.summary && !op.description) op.description = op.summary;
		op.summary = title(op.operationId);
		const body = samples[op.operationId];
		const source = body === null ? signIn : client + body;
		op['x-codeSamples'] = [{ lang: 'csharp', label: 'C# SDK', source: 'using OhrTech.Management;\n\n' + source }];
	}
}
if (missing.length) {
	console.error(`sync-openapi: add a C# sample for ${missing.join(', ')} in scripts/sync-openapi.mjs`);
	process.exit(1);
}

await mkdir(new URL('.', out), { recursive: true });
await writeFile(out, JSON.stringify(doc, null, 2) + '\n');
console.log(`sync-openapi: ${Object.keys(samples).length} operations, written to public/management-api/openapi.json`);

// The same samples, as one C# file CI compiles against the published SDK (examples/reference-samples.cs).
const compiled = Object.entries(samples)
	.filter(([, s]) => s !== null)
	.map(([id, s]) => `static async Task ${id}(OhrManagementClient api, string adbPublicKey, string clientKey)\n{\n${s.replace(/^/gm, '    ')}\n}`)
	.join('\n\n');
await writeFile(
	new URL('../examples/reference-samples.cs', import.meta.url),
	`#:package OhrTech.Management@*\n// Written by scripts/sync-openapi.mjs: the API reference's C# samples, compiled in CI. Do not edit.\n` +
		`#pragma warning disable CS8321, CS0168, CS8625, CS0219\nusing OhrTech.Management;\n\n` +
		`await Task.CompletedTask;\n\n${compiled}\n\nstatic async Task SignIn()\n{\n${signIn.replace(/^/gm, '    ')}\n}\n`,
);
