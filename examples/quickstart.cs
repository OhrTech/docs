#:package OhrTech.Management@*
// Lists your devices, then shows a message on one of them.
//   OHR_TOKEN=ohrs_… dotnet run quickstart.cs            list devices
//   OHR_TOKEN=ohrs_… dotnet run quickstart.cs -- 42      …and show a message on device 42
using OhrTech.Management;

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

foreach (var d in await api.ListDevicesAsync())
{
    var state = d.Online ? "online" : "offline";
    var battery = d.BatteryLevel is { } level ? $"{level}%" : "–";
    Console.WriteLine($"{d.DeviceId,6}  {d.Serial,-16} {d.FleetName ?? "(no fleet)",-24} {state,-8} battery {battery}");
}

if (args.Length > 0 && long.TryParse(args[0], out var deviceId))
{
    var task = await api.ShowMessageAsync(deviceId, new ShowMessageRequest { Text = "Hello from the Management API" });
    Console.WriteLine($"Sent: task {task.TaskId} is {task.Status}");
}
