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

static async Task GetFleetAppPolicy(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var policy = await api.GetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy");
}

static async Task SetFleetAppPolicy(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // Stay on the version the fleet runs now, until you approve another:
    var policy = await api.SetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy", new SetAppPolicyRequest { Mode = "manual" });
    // Later, approve the newest:
    policy = await api.SetFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy",
        new SetAppPolicyRequest { Mode = "manual", ApprovedVersionCode = policy.Waiting[0].VersionCode });
}

static async Task ClearFleetAppPolicy(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var policy = await api.ClearFleetAppPolicyAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_3k7fx2qd0m9b4tz1s8c6ve5hwy");
}

static async Task GetUpdateTiming(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timing = await api.GetUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb");
}

static async Task SetUpdateTiming(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timing = await api.SetUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb", new UpdateTimingRequest
    {
        Mode = "window", Start = "02:00", End = "05:00", Days = [1, 2, 3, 4, 5],
    });
}

static async Task ClearUpdateTiming(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timing = await api.ClearUpdateTimingAsync("flt_2dn47a2xa58j1txv8m28spkmyb");
}

static async Task ListAppPermissions(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var permissions = await api.ListAppPermissionsAsync("app_44n36syg958kyv8gzmq6f59tkq");
    foreach (var p in permissions.Where(p => p.State == "waiting"))
        Console.WriteLine($"{p.Permission} waits for OhrTech's approval");
}

static async Task ListOrgPermissionApprovals(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var approved = await api.ListOrgPermissionApprovalsAsync("org_06b160s9zq89js04rhtj7zs0ke");
}

static async Task ApproveOrgPermission(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // OhrTech staff only.
    var approval = await api.ApproveOrgPermissionAsync("org_06b160s9zq89js04rhtj7zs0ke", "android.permission.CHANGE_DEVICE_IDLE_TEMP_WHITELIST",
        new ApprovePermissionRequest { Note = "Push: wake the app for incoming calls" });
}

static async Task WithdrawOrgPermission(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // OhrTech staff only.
    await api.WithdrawOrgPermissionAsync("org_06b160s9zq89js04rhtj7zs0ke", "android.permission.CHANGE_DEVICE_IDLE_TEMP_WHITELIST");
}

static async Task ApproveAppPermission(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // OhrTech staff only, for OhrTech's apps.
    var approval = await api.ApproveAppPermissionAsync("app_3k7fx2qd0m9b4tz1s8c6ve5hwy", "android.permission.CHANGE_DEVICE_IDLE_TEMP_WHITELIST");
}

static async Task WithdrawAppPermission(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // OhrTech staff only, for OhrTech's apps.
    await api.WithdrawAppPermissionAsync("app_3k7fx2qd0m9b4tz1s8c6ve5hwy", "android.permission.CHANGE_DEVICE_IDLE_TEMP_WHITELIST");
}

static async Task GetAppConfigSchema(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var schema = await api.GetAppConfigSchemaAsync("app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task GetFleetAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.GetFleetAppConfigAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetFleetAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.SetFleetAppConfigAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new SetAppConfigRequest
    {
        Values = new()
        {
            ["server_url"] = "https://orders.example.com",
            ["store_number"] = 42,
        },
    });
}

static async Task ClearFleetAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.ClearFleetAppConfigAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task GetDeviceAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.GetDeviceAppConfigAsync("dev_3swt6f457299btzpw1bxx4wf0q", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetDeviceAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.SetDeviceAppConfigAsync("dev_3swt6f457299btzpw1bxx4wf0q", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new SetAppConfigRequest
    {
        Values = new() { ["store_number"] = 17 },
    });
}

static async Task ClearDeviceAppConfig(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var config = await api.ClearDeviceAppConfigAsync("dev_3swt6f457299btzpw1bxx4wf0q", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task GetDeviceIdentityRoot(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rootPem = await api.GetDeviceIdentityRootAsync();
}

static async Task CreateIssuingCa(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var ca = await api.CreateIssuingCaAsync("org_1g8kq3zr5m2x9v4c7b6n0p3t8w", new CreateIssuingCaRequest { Name = "Stores", LeafDays = 90 });
    // Sign ca.CsrPem with your client root, then upload the result with UploadIssuingCaCertificateAsync.
}

static async Task ListIssuingCas(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var cas = await api.ListIssuingCasAsync("org_1g8kq3zr5m2x9v4c7b6n0p3t8w");
}

static async Task UploadIssuingCaCertificate(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var ca = await api.UploadIssuingCaCertificateAsync("org_1g8kq3zr5m2x9v4c7b6n0p3t8w", "ica_4x8n2q7m5k1v9c3b6z0p8r2t5w", new UploadOrgCertificateRequest
    {
        CertificatePem = File.ReadAllText("stores-issuing-ca.pem"),
        ChainPem = File.ReadAllText("client-root.pem"),
    });
}

static async Task RevokeIssuingCa(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var ca = await api.RevokeIssuingCaAsync("org_1g8kq3zr5m2x9v4c7b6n0p3t8w", "ica_4x8n2q7m5k1v9c3b6z0p8r2t5w");
}

static async Task GetAppCertificates(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var certificates = await api.GetAppCertificatesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetAppCertificates(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var certificates = await api.SetAppCertificatesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new SetAppCertificatesRequest
    {
        IssuingCaId = "ica_4x8n2q7m5k1v9c3b6z0p8r2t5w",
        Trust = [File.ReadAllText("private-ca.pem")],
    });
}

static async Task ClearAppCertificates(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var certificates = await api.ClearAppCertificatesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task ListDeviceCertificates(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var certificates = await api.ListDeviceCertificatesAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ListAppCrashes(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var crashes = await api.ListAppCrashesAsync("app_7h2m9q4x1c8v3k5n6p0r2s4t6w", days: 7);
    foreach (var c in crashes) Console.WriteLine($"{c.Count} x {c.Title}");
}

static async Task ListCrashReports(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var reports = await api.ListCrashReportsAsync("app_7h2m9q4x1c8v3k5n6p0r2s4t6w", "3f9c1a7e5b2d8c40", limit: 10);
}

static async Task ListDeviceCrashes(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var crashes = await api.ListDeviceCrashesAsync("dev_3swt6f457299btzpw1bxx4wf0q", limit: 20);
}

static async Task VerifyAttestation(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    byte[] challenge = new byte[32];      // the challenge your server gave the app
    List<string> ec = [];                  // the app's Attestation.toJson()["ec"]: the chain, base64 DER
    var verdict = await api.VerifyAttestationAsync(new VerifyAttestationRequest
    {
        Challenge = Convert.ToBase64String(challenge),
        Ec = ec,
        Package = "com.example.kiosk",
        SignerSha256 = "5f0c3a9e7d21b8e64f9a0c2d1e3b5a7f6c8d0e2f4a6b8c0d2e4f6a8b0c2d4e6f",
    });
    var trusted = verdict.Valid && verdict.Problems.Count == 0;
}

static async Task RevokeDeviceAttestation(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var revoked = await api.RevokeDeviceAttestationAsync("dev_3swt6f457299btzpw1bxx4wf0q", new RevokeAttestationRequest { Reason = "lost" });
}

static async Task GetAttestationRoot(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var rootPem = await api.GetAttestationRootAsync();
}

static async Task GetAttestationStatus(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var statusJws = await api.GetAttestationStatusAsync();
    // Or, checked: OhrTech.Management.Attestation.AttestationStatusList.FetchAsync(http)
}

static async Task GetSdkAccess(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var access = await api.GetSdkAccessAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetSdkAccess(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var access = await api.SetSdkAccessAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new SetSdkAccessRequest { Areas = ["wifi", "kiosk"] });
}

static async Task ClearSdkAccess(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var access = await api.ClearSdkAccessAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task VerifyKeyAttestation(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // The fields of the app's OhrKeys.attest() result, as the app sent them.
    var keyAttestation = new KeyAttestationDto
    {
        Alg = "x-wing", PublicKey = "<base64>", Owner = "<owner>", Challenge = "<base64>", CoseSign1 = "<base64>", DiceChain = "<base64>",
    };
    var verdict = await api.VerifyKeyAttestationAsync("dev_3swt6f457299btzpw1bxx4wf0q", keyAttestation);
    if (!verdict.Valid) throw new UnauthorizedAccessException(verdict.Reason);
}

static async Task GetUsbDevices(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var usb = await api.GetUsbDevicesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetUsbDevices(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var usb = await api.SetUsbDevicesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new SetUsbDevicesRequest
    {
        Devices =
        [
            System.Text.Json.Nodes.JsonNode.Parse("""{"vendorId": "0x05e0", "productId": "0x1200", "name": "Barcode scanner"}""")!,
            System.Text.Json.Nodes.JsonNode.Parse("""{"manufacturer": "Ingenico", "model": "Lane/3000", "name": "Payment terminal"}""")!,
        ],
    });
}

static async Task ClearUsbDevices(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var usb = await api.ClearUsbDevicesAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task GetResponseTimeout(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timeout = await api.GetResponseTimeoutAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task SetResponseTimeout(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timeout = await api.SetResponseTimeoutAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w",
        new SetResponseTimeoutRequest { Seconds = 30 });
}

static async Task ClearResponseTimeout(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var timeout = await api.ClearResponseTimeoutAsync("flt_2dn47a2xa58j1txv8m28spkmyb", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w");
}

static async Task ListDevicePeripherals(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var peripherals = await api.ListDevicePeripheralsAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ListDeviceEvents(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var events = await api.ListDeviceEventsAsync("dev_3swt6f457299btzpw1bxx4wf0q", limit: 50);
}

static async Task RequestLogBundle(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var bundle = await api.RequestLogBundleAsync("dev_3swt6f457299btzpw1bxx4wf0q", "app_7h2m9q4x1c8v3k5n6p0r2s4t6w", new RequestLogBundleRequest { AllowMobileData = false });
}

static async Task ListLogBundles(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var bundles = await api.ListLogBundlesAsync("dev_3swt6f457299btzpw1bxx4wf0q");
    // Each uploaded one has a DownloadUrl, good for an hour.
}

static async Task SendPush(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    // From the app: its token, the token's suite and public key. Yours: the sender key the app trusts.
    string token = "psh_4n7q2x9m3k8v1c5b0z6p2r7t4w", suite = "x25519", appPublicKey = "<the token's public key, base64>";
    var sender = OhrTech.Management.Push.PushSenderKey.Import(Convert.FromBase64String(Environment.GetEnvironmentVariable("PUSH_ED25519_SEED")!), null);
    var message = OhrTech.Management.Push.PushSealer.SealBase64(token, suite, Convert.FromBase64String(appPublicKey), sender,
        "order-1234", DateTimeOffset.UtcNow.AddHours(1), "{\"order\": 1234}"u8.ToArray());
    var result = await api.SendPushAsync(new PushRequest
    {
        Messages = [new PushRequestMessage { Token = token, Message = message, Priority = "high" }],
    });
}

static async Task GetPushStatus(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var status = await api.GetPushStatusAsync("pmsg_0km64jzn2t8qrb11c8t9x8abdp");
}

static async Task ListPushTokens(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var tokens = await api.ListPushTokensAsync("dev_3swt6f457299btzpw1bxx4wf0q");
}

static async Task ExitKiosk(OhrManagementClient api, string adbPublicKey, string clientKey)
{
    var task = await api.ExitKioskAsync("dev_3swt6f457299btzpw1bxx4wf0q", new ExitKioskRequest { Minutes = 15 });
}

static async Task SignIn()
{
    // The SDK runs the device code flow and refreshes the access token for you.
    var session = await OhrUserSession.SignInAsync(p =>
        Console.WriteLine($"Open {p.VerificationUri} and enter {p.UserCode}"));
    using var api = OhrManagementClient.ForUser(session);
}
