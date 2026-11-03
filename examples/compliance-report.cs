#:package OhrTech.Management@*
// Writes a CSV of every device's state against your bar: checked in recently, a recent security
// patch, settings applied, the OhrTech Agent up to date and, optionally, your app installed. Open it in a spreadsheet or
// attach it to your audit evidence.
//   OHR_TOKEN=ohrs_… dotnet run compliance-report.cs > devices.csv
//   OHR_TOKEN=ohrs_… dotnet run compliance-report.cs -- com.example.yourapp > devices.csv
// Needs ViewDevices.
using OhrTech.Management;

var requiredApp = args.FirstOrDefault();
var checkInWithin = TimeSpan.FromHours(24);
var patchWithin = TimeSpan.FromDays(90);

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

Console.WriteLine("device,serial,fleet,last_check_in,os_build,security_patch,agent,agent_current,settings_in_sync," +
                  "settings_errors,app_version,compliant,reasons");
foreach (var summary in await api.ListDevicesAsync())
{
    var d = await api.GetDeviceAsync(summary.DeviceId);
    var reasons = new List<string>();

    if (d.LastCheckIn is not { } seen || DateTimeOffset.UtcNow - seen > checkInWithin) reasons.Add("not checked in for 24 hours");
    if (!DateTime.TryParse(d.Os.SecurityPatch, out var patch)) reasons.Add("security patch not reported");
    else if (DateTime.UtcNow - patch > patchWithin) reasons.Add("security patch over 90 days old");
    if (d.Settings.InSync != true) reasons.Add("settings not applied yet");
    if (d.Settings.Errors > 0) reasons.Add($"{d.Settings.Errors} setting(s) failed");
    if (d.Agent.Outdated) reasons.Add("Agent update pending");

    string? appVersion = null;
    if (requiredApp != null)
    {
        appVersion = (await api.ListInstalledAppsAsync(d.DeviceId)).FirstOrDefault(a => a.Package == requiredApp)?.Version;
        if (appVersion == null) reasons.Add($"{requiredApp} not installed");
    }

    Console.WriteLine(string.Join(",",
        d.DeviceId, Csv(d.Serial), Csv(d.FleetName), d.LastCheckIn?.ToString("u"), Csv(d.Os.Fingerprint),
        Csv(d.Os.SecurityPatch), d.Agent.Version, !d.Agent.Outdated, d.Settings.InSync, d.Settings.Errors, Csv(appVersion),
        reasons.Count == 0, Csv(string.Join("; ", reasons))));
}

static string Csv(string? value) =>
    value is null ? "" : value.IndexOfAny([',', '"', '\n']) < 0 ? value : $"\"{value.Replace("\"", "\"\"")}\"";
