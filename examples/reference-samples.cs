#:package OhrTech.Management@*
// Written by scripts/sync-openapi.mjs: the API reference's C# samples, compiled in CI. Do not edit.
#pragma warning disable CS8321, CS0168, CS8625, CS0219
using OhrTech.Management;

await Task.CompletedTask;

static async Task GetMe(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var me = await api.GetMeAsync();
}

static async Task ListDevices(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var devices = await api.ListDevicesAsync(online: true);
}

static async Task GetDevice(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var device = await api.GetDeviceAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task MoveDevice(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.MoveDeviceAsync("dev_3swt6f457299btzpw1bxx4wf0q", new MoveDeviceRequest { FleetId = "flt_2dn47a2xa58j1txv8m28spkmyb" });
}

static async Task ListEnrolments(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var enrolments = await api.ListEnrolmentsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ListInstalledApps(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var apps = await api.ListInstalledAppsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task RequestCheckIn(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var checkIn = await api.RequestCheckInAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ListFleets(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var fleets = await api.ListFleetsAsync();
}

static async Task ListAdbSessions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessions = await api.ListAdbSessionsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task OpenAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // The ohr command line tool opens ADB sessions and runs the encrypted tunnel:
    //   ohr adb connect A0000A00042
    var session = await api.OpenAdbSessionAsync("dev_3swt6f457299btzpw1bxx4wf0q", new OpenAdbSessionRequest
    {
        AdbPublicKey = adbPublicKey,
        ClientKey = clientKey,
    });
}

static async Task GetAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var session = await api.GetAdbSessionAsync("adb_1w6k4wmxxv91bamfgcm4xhn42h");
}

static async Task CloseAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.CloseAdbSessionAsync("adb_1w6k4wmxxv91bamfgcm4xhn42h");
}

static async Task ListRemoteControlSessions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessions = await api.ListRemoteControlSessionsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task OpenRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rc = await api.OpenRemoteControlSessionAsync("dev_3swt6f457299btzpw1bxx4wf0q", new OpenRemoteControlRequest
    {
        EmbedOrigins = ["https://support.example.com"],
    });
    // <iframe src="{rc.Embed.Url}" allow="fullscreen; clipboard-read"></iframe>
}

static async Task GetRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var session = await api.GetRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");
}

static async Task CloseRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.CloseRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");
}

static async Task JoinRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rc = await api.JoinRemoteControlSessionAsync("rc_1shbwr5f2984fagzq5pd22kjyj");
}

static async Task GetSettingsCatalog(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var catalog = await api.GetSettingsCatalogAsync();
}

static async Task GetDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.GetDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ReplaceDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.ReplaceDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PolicyDto
    {
        Settings = new()
        {
            ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },
        }
    });
}

static async Task UpdateDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.UpdateDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PolicyPatchDto
    {
        Settings = new()
        {
            ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },
            ["dev.usb_debugging"] = null, // remove the override
        }
    });
}

static async Task PushDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.PushDeviceSettingsAsync("dev_3swt6f457299btzpw1bxx4wf0q", new PushSettingsRequest { Reapply = ["display.brightness"] });
}

static async Task ListTasks(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var tasks = await api.ListTasksAsync("dev_3swt6f457299btzpw1bxx4wf0q", limit: 50);
}

static async Task CreateTask(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.CreateTaskAsync("dev_3swt6f457299btzpw1bxx4wf0q", new CreateTaskRequest
    {
        TaskClass = "ShowToast",
        Parameters = ["Please plug me in"],
    });
}

static async Task GetTask(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.GetTaskAsync("dev_3swt6f457299btzpw1bxx4wf0q", "tsk_3b0wkr78p69s7tzaevqeef8pyh");
}

static async Task ShowMessage(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.ShowMessageAsync("dev_3swt6f457299btzpw1bxx4wf0q", new ShowMessageRequest { Text = "Please plug me in" });
}

static async Task RunShellCommand(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.RunShellCommandAsync("dev_3swt6f457299btzpw1bxx4wf0q", new RunShellCommandRequest { Command = "am start -a android.settings.SETTINGS" });
    task = await api.WaitForTaskAsync(task);
    Console.WriteLine($"{task.Shell?.ExitCode}: {task.Shell?.Stdout}");
}

static async Task ListApps(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var apps = await api.ListAppsAsync(package: "com.example.kiosk");
}

static async Task CreateApp(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var app = await api.CreateAppAsync(new CreateAppRequest { PackageName = "com.example.kiosk", Name = "Kiosk" });
}

static async Task GetApp(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var app = await api.GetAppAsync("app_44n36syg958kyv8gzmq6f59tkq");
}

static async Task UpdateApp(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var app = await api.UpdateAppAsync("app_44n36syg958kyv8gzmq6f59tkq", new UpdateAppRequest { Name = "Store kiosk" });
}

static async Task ListAppVersions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var versions = await api.ListAppVersionsAsync("app_44n36syg958kyv8gzmq6f59tkq");
}

static async Task UploadApk(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // The ohr command line does the same: ohr apps upload com.example.kiosk app-release.apk --enforce --wait
    var version = await api.UploadApkAsync("app_44n36syg958kyv8gzmq6f59tkq", "app-release.apk", state: "enforced");
}

static async Task SetAppVersionState(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var version = await api.SetAppVersionStateAsync("app_44n36syg958kyv8gzmq6f59tkq", 240, new SetAppVersionStateRequest { State = "enforced" });
}

static async Task GetAppRollout(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rollout = await api.GetAppRolloutAsync("app_44n36syg958kyv8gzmq6f59tkq");
    // Or wait until every online device runs the enforced version:
    rollout = await api.WaitForRolloutAsync("app_44n36syg958kyv8gzmq6f59tkq");
}

static async Task ListFleetApps(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var apps = await api.ListFleetAppsAsync("flt_2dn47a2xa58j1txv8m28spkmyb");
}

static async Task AddFleetApp(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.AddFleetAppAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_44n36syg958kyv8gzmq6f59tkq");
}

static async Task RemoveFleetApp(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.RemoveFleetAppAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_44n36syg958kyv8gzmq6f59tkq");
}

static async Task SignIn()
{
    // The SDK runs the device code flow and refreshes the access token for you.
    var session = await OhrUserSession.SignInAsync(p =>
        Console.WriteLine($"Open {p.VerificationUri} and enter {p.UserCode}"));
    using var api = OhrManagementClient.ForUser(session);
}
