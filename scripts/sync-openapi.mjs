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

// One C# call per operationId, in the SDK's terms (OhrTech.Management on NuGet).
const samples = {
	GetMe: 'var me = await api.GetMeAsync();',
	ListDevices: 'var devices = await api.ListDevicesAsync(online: true);',
	GetDevice: 'var device = await api.GetDeviceAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	MoveDevice: 'await api.MoveDeviceAsync("dev_3swt6f457299btzpw1bxx4wf0q", new MoveDeviceRequest { FleetId = "flt_2dn47a2xa58j1txv8m28spkmyb" });',
	ListEnrolments: 'var enrolments = await api.ListEnrolmentsAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	ListInstalledApps: 'var apps = await api.ListInstalledAppsAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	RequestCheckIn: 'var checkIn = await api.RequestCheckInAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	ListFleets: 'var fleets = await api.ListFleetsAsync();',
	ListAdbSessions: 'var sessions = await api.ListAdbSessionsAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	OpenAdbSession:
		'// The ohr command line tool opens ADB sessions and runs the encrypted tunnel:\n//   ohr adb connect A0000A00042\n' +
		'var session = await api.OpenAdbSessionAsync("dev_3swt6f457299btzpw1bxx4wf0q", new OpenAdbSessionRequest\n{\n    AdbPublicKey = adbPublicKey,\n    ClientKey = clientKey,\n});',
	GetAdbSession: 'var session = await api.GetAdbSessionAsync("adb_1w6k4wmxxv91bamfgcm4xhn42h");',
	CloseAdbSession: 'await api.CloseAdbSessionAsync("adb_1w6k4wmxxv91bamfgcm4xhn42h");',
	ListRemoteControlSessions: 'var sessions = await api.ListRemoteControlSessionsAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	OpenRemoteControlSession:
		'var rc = await api.OpenRemoteControlSessionAsync("dev_3swt6f457299btzpw1bxx4wf0q", new OpenRemoteControlRequest\n{\n    EmbedOrigins = ["https://support.example.com"],\n});\n// <iframe src="{rc.Embed.Url}" allow="fullscreen; clipboard-read"></iframe>',
	GetRemoteControlSession: 'var session = await api.GetRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");',
	CloseRemoteControlSession: 'await api.CloseRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");',
	JoinRemoteControlSession: 'var rc = await api.JoinRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");',
	GetSettingsCatalog: 'var catalog = await api.GetSettingsCatalogAsync();',
	GetDeviceSettings: 'var settings = await api.GetDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q");',
	ReplaceDeviceSettings:
		'var settings = await api.ReplaceDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PolicyDto\n{\n    Settings = new()\n    {\n        ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },\n    }\n});',
	UpdateDeviceSettings:
		'var settings = await api.UpdateDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PolicyPatchDto\n{\n    Settings = new()\n    {\n        ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },\n        ["dev.usb_debugging"] = null, // remove the override\n    }\n});',
	PushDeviceSettings:
		'await api.PushDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PushSettingsRequest { Reapply = ["display.brightness"] });',
	StartDeviceLogin: null, // the SDK runs the whole device code flow:
	PollDeviceLogin: null,
	RefreshLogin: null,
	ListTasks: 'var tasks = await api.ListTasksAsync("dev_3swt6f457299btzpw1bxx4wf0q", limit: 50);',
	CreateTask:
		'var task = await api.CreateTaskAsync("dev_3swt6f457299btzpw1bxx4wf0q", new CreateTaskRequest\n{\n    TaskClass = "ShowToast",\n    Parameters = ["Please plug me in"],\n});',
	GetTask: 'var task = await api.GetTaskAsync("dev_3swt6f457299btzpw1bxx4wf0q", "tsk_3b0wkr78p69s7tzaevqeef8pyh");',
	ShowMessage: 'var task = await api.ShowMessageAsync("dev_3swt6f457299btzpw1bxx4wf0q", new ShowMessageRequest { Text = "Please plug me in" });',
	RunShellCommand:
		'var task = await api.RunShellCommandAsync("dev_3swt6f457299btzpw1bxx4wf0q", new RunShellCommandRequest { Command = "am start -a android.settings.SETTINGS" });\n' +
		'task = await api.WaitForTaskAsync(task);\nConsole.WriteLine($"{task.Shell?.ExitCode}: {task.Shell?.Stdout}");',
	ListApps: 'var apps = await api.ListAppsAsync(package: "com.example.kiosk");',
	CreateApp: 'var app = await api.CreateAppAsync(new CreateAppRequest { PackageName = "com.example.kiosk", Name = "Kiosk" });',
	GetApp: 'var app = await api.GetAppAsync("app_44n36syg958kyv8gzmq6f59tkq");',
	UpdateApp: 'var app = await api.UpdateAppAsync("app_44n36syg958kyv8gzmq6f59tkq", new UpdateAppRequest { Name = "Store kiosk" });',
	ListAppVersions: 'var versions = await api.ListAppVersionsAsync("app_44n36syg958kyv8gzmq6f59tkq");',
	UploadApk:
		'// The ohr command line does the same: ohr apps upload com.example.kiosk app-release.apk --enforce --wait\n' +
		'var version = await api.UploadApkAsync("app_44n36syg958kyv8gzmq6f59tkq", "app-release.apk", state: "enforced");',
	SetAppVersionState:
		'var version = await api.SetAppVersionStateAsync("app_44n36syg958kyv8gzmq6f59tkq", 240, new SetAppVersionStateRequest { State = "enforced" });',
	GetAppRollout:
		'var rollout = await api.GetAppRolloutAsync("app_44n36syg958kyv8gzmq6f59tkq");\n// Or wait until every online device runs the enforced version:\nrollout = await api.WaitForRolloutAsync("app_44n36syg958kyv8gzmq6f59tkq");',
	ListFleetApps: 'var apps = await api.ListFleetAppsAsync("flt_2dn47a2xa58j1txv8m28spkmyb");',
	AddFleetApp: 'await api.AddFleetAppAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_44n36syg958kyv8gzmq6f59tkq");',
	RemoveFleetApp: 'await api.RemoveFleetAppAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_44n36syg958kyv8gzmq6f59tkq");',
	GetFleetAppPolicy: 'var policy = await api.GetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy");',
	SetFleetAppPolicy:
		'// Stay on the version the fleet runs now, until you approve another:\n' +
		'var policy = await api.SetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy", new SetAppPolicyRequest { Mode = "manual" });\n' +
		'// Later, approve the newest:\n' +
		'policy = await api.SetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy",\n' +
		'    new SetAppPolicyRequest { Mode = "manual", ApprovedVersionCode = policy.Waiting[0].VersionCode });',
	ClearFleetAppPolicy: 'var policy = await api.ClearFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy");',
	GetUpdateTiming: 'var timing = await api.GetUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb");',
	SetUpdateTiming:
		'var timing = await api.SetUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb", new UpdateTimingRequest\n{\n' +
		'    Mode = "window", Start = "02:00", End = "05:00", Days = [1, 2, 3, 4, 5],\n});',
	ClearUpdateTiming: 'var timing = await api.ClearUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb");',
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
	RunShellCommand: 'Run a shell command',
	UploadApk: 'Upload an APK',
	SetAppVersionState: "Set a version's state",
	GetAppRollout: 'Follow a rollout',
	ListFleetApps: "List a fleet's apps",
	AddFleetApp: 'Assign an app to a fleet',
	RemoveFleetApp: 'Unassign an app from a fleet',
	GetFleetAppPolicy: "A fleet's update policy for an OhrTech app",
	SetFleetAppPolicy: 'Choose automatic or manual, or approve a version',
	ClearFleetAppPolicy: "Follow the parent fleet's policy",
	GetUpdateTiming: 'When a fleet installs updates',
	SetUpdateTiming: 'Set when a fleet installs updates',
	ClearUpdateTiming: "Follow the parent fleet's timing",
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
