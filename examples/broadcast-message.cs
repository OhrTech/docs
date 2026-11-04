#:package OhrTech.Management@*
// Shows a message on every device in a fleet and the fleets below it.
//   OHR_TOKEN=ohrs_… dotnet run broadcast-message.cs -- "Downtown store" "We close in 15 minutes."
// Needs ViewDevices and SendMessages. A device that is offline shows it when it next checks in.
using OhrTech.Management;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: broadcast-message.cs <fleet name> <message>");
    return 2;
}
var (fleetName, text) = (args[0], args[1]);

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

var fleets = await api.ListFleetsAsync();
var root = fleets.SingleOrDefault(f => string.Equals(f.FleetName, fleetName, StringComparison.OrdinalIgnoreCase));
if (root == null)
{
    Console.Error.WriteLine($"No fleet named \"{fleetName}\".");
    return 1;
}

// The fleet and every fleet below it.
var inFleet = new HashSet<string> { root.FleetId };
for (var grew = true; grew;)
{
    grew = false;
    foreach (var f in fleets)
        if (f.ParentFleetId is { } parent && inFleet.Contains(parent) && inFleet.Add(f.FleetId))
            grew = true;
}

int sent = 0, failed = 0;
foreach (var device in await api.ListDevicesAsync())
{
    if (device.FleetId is not { } fleetId || !inFleet.Contains(fleetId)) continue;
    try
    {
        await api.ShowMessageAsync(device.DeviceId, new ShowMessageRequest { Text = text });
        sent++;
    }
    catch (OhrApiException<ProblemDetails> e)
    {
        Console.Error.WriteLine($"{device.Serial ?? device.DeviceId}: {e.Result.Title}");
        failed++;
    }
}

Console.WriteLine($"Sent to {sent} device(s){(failed > 0 ? $", {failed} failed" : "")}.");
return failed > 0 ? 1 : 0;
