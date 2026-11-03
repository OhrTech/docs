#:package OhrTech.Management@1.0.1
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
    var device = await api.GetDeviceAsync(42);
}

static async Task MoveDevice(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    await api.MoveDeviceAsync(42, new MoveDeviceRequest { FleetId = 7 });
}

static async Task ListEnrolments(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var enrolments = await api.ListEnrolmentsAsync(42);
}

static async Task ListInstalledApps(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var apps = await api.ListInstalledAppsAsync(42);
}

static async Task RequestCheckIn(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var checkIn = await api.RequestCheckInAsync(42);
}

static async Task ListFleets(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var fleets = await api.ListFleetsAsync();
}

static async Task ListAdbSessions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessions = await api.ListAdbSessionsAsync(42);
}

static async Task OpenAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // The ohr command line tool opens ADB sessions and runs the encrypted tunnel:
    //   ohr adb connect 42
    var session = await api.OpenAdbSessionAsync(42, new OpenAdbSessionRequest
    {
        AdbPublicKey = adbPublicKey,
        ClientKey = clientKey,
    });
}

static async Task GetAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");
    var session = await api.GetAdbSessionAsync(sessionId);
}

static async Task CloseAdbSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");
    await api.CloseAdbSessionAsync(sessionId);
}

static async Task ListRemoteControlSessions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessions = await api.ListRemoteControlSessionsAsync(42);
}

static async Task OpenRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rc = await api.OpenRemoteControlSessionAsync(42, new OpenRemoteControlRequest
    {
        EmbedOrigins = ["https://support.example.com"],
    });
    // <iframe src="{rc.Embed.Url}" allow="fullscreen; clipboard-read"></iframe>
}

static async Task GetRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");
    var session = await api.GetRemoteControlSessionAsync(sessionId);
}

static async Task CloseRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");
    await api.CloseRemoteControlSessionAsync(sessionId);
}

static async Task JoinRemoteControlSession(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var sessionId = Guid.Parse("8a3c6f0e-2d41-4b8e-9f6a-1c2d3e4f5a6b");
    var rc = await api.JoinRemoteControlSessionAsync(sessionId);
}

static async Task GetSettingsCatalog(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var catalog = await api.GetSettingsCatalogAsync();
}

static async Task GetDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.GetDeviceSettingsAsync(42);
}

static async Task ReplaceDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.ReplaceDeviceSettingsAsync(42, new PolicyDto
    {
        Settings = new()
        {
            ["display.brightness"] = new SettingPolicy { Mode = SettingMode.Locked, Value = 80 },
        }
    });
}

static async Task UpdateDeviceSettings(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var settings = await api.UpdateDeviceSettingsAsync(42, new PolicyPatchDto
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
    await api.PushDeviceSettingsAsync(42, new PushSettingsRequest { Reapply = ["display.brightness"] });
}

static async Task ListTasks(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var tasks = await api.ListTasksAsync(42, limit: 50);
}

static async Task CreateTask(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.CreateTaskAsync(42, new CreateTaskRequest
    {
        TaskClass = "ShowToast",
        Parameters = ["Please plug me in"],
    });
}

static async Task GetTask(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.GetTaskAsync(42, 9131);
}

static async Task ShowMessage(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.ShowMessageAsync(42, new ShowMessageRequest { Text = "Please plug me in" });
}

static async Task SignIn()
{
    // The SDK runs the device code flow and refreshes the access token for you.
    var session = await OhrUserSession.SignInAsync(p =>
        Console.WriteLine($"Open {p.VerificationUri} and enter {p.UserCode}"));
    using var api = OhrManagementClient.ForUser(session);
}
