#:package OhrTech.Management@*
// Lists your devices, then shows a message on one of them.
//   OHR_TOKEN=ohrs_… dotnet run quickstart.cs            list devices
//   OHR_TOKEN=ohrs_… dotnet run quickstart.cs -- DEVICE  …and show a message on that device (its id)
using OhrTech.Management;

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

foreach (var d in await api.ListDevicesAsync())
{
    var state = d.Online ? "online" : "offline";
    var battery = d.BatteryLevel is { } level ? $"{level}%" : "–";
    Console.WriteLine($"{d.DeviceId}  {d.Serial,-16} {d.FleetName ?? "(no fleet)",-24} {state,-8} battery {battery}");
}

if (args.Length > 0)
{
    var task = await api.ShowMessageAsync(args[0], new ShowMessageRequest { Text = "Hello from the Management API" });
    Console.WriteLine($"Sent: task {task.TaskId} is {task.Status}");
}
