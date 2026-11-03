#:package OhrTech.Management@*
// Posts a summary of devices that need attention to a Slack or Microsoft Teams channel.
// Run it on a schedule (cron, a scheduled task, a GitHub Actions schedule).
//   OHR_TOKEN=ohrs_… ALERT_WEBHOOK_URL=https://hooks.slack.com/… dotnet run fleet-health-alerts.cs
// Needs ViewDevices. Without ALERT_WEBHOOK_URL it prints the summary instead.
using System.Text;
using System.Text.Json.Nodes;
using OhrTech.Management;

var offlineAfter = TimeSpan.FromHours(6);
const int lowBattery = 15;

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

var alerts = new List<string>();
foreach (var d in await api.ListDevicesAsync())
{
    var name = $"{d.Serial ?? $"device {d.DeviceId}"} ({d.FleetName ?? "no fleet"})";

    if (d.LastCheckIn is { } seen && DateTimeOffset.UtcNow - seen is var away && away > offlineAfter)
        alerts.Add($"{name} has not checked in for {(away.TotalDays >= 2 ? $"{away.TotalDays:0} days" : $"{away.TotalHours:0} hours")}");
    if (d.BatteryLevel is < lowBattery && !d.Charging)
        alerts.Add($"{name} is at {d.BatteryLevel}% battery and not charging");
    if (d.SettingsErrors > 0)
        alerts.Add($"{name} could not apply {d.SettingsErrors} setting(s)");
    if (d.AgentOutdated)
        alerts.Add($"{name} is waiting to update the OhrTech Agent");
}

if (alerts.Count == 0) return;

var text = $"{alerts.Count} device alert(s):\n• " + string.Join("\n• ", alerts);
var webhook = Environment.GetEnvironmentVariable("ALERT_WEBHOOK_URL");
if (string.IsNullOrEmpty(webhook))
{
    Console.WriteLine(text);
    return;
}

// Slack incoming webhooks and Teams workflow webhooks both accept {"text": "…"}.
using var http = new HttpClient();
var body = new JsonObject { ["text"] = text }.ToJsonString();
(await http.PostAsync(webhook, new StringContent(body, Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode();
