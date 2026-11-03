#:package OhrTech.Management@1.0.1
// Puts devices into fleets from your own inventory: a CSV of serial number and fleet name.
//   OHR_TOKEN=ohrs_… dotnet run sort-into-fleets.cs -- inventory.csv           show what would move
//   OHR_TOKEN=ohrs_… dotnet run sort-into-fleets.cs -- inventory.csv --apply   move them
// Needs ViewDevices and MoveDevices.
using OhrTech.Management;

var file = args.FirstOrDefault(a => !a.StartsWith("--")) ?? "inventory.csv";
var apply = args.Contains("--apply");

using var api = OhrManagementClient.ForServiceAccount(
    Environment.GetEnvironmentVariable("OHR_TOKEN") ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token."));

var fleets = (await api.ListFleetsAsync()).ToLookup(f => f.FleetName, StringComparer.OrdinalIgnoreCase);
var devices = (await api.ListDevicesAsync())
    .Where(d => d.Serial != null)
    .ToDictionary(d => d.Serial!, StringComparer.OrdinalIgnoreCase);

int moved = 0, unchanged = 0, problems = 0;
foreach (var line in File.ReadLines(file).Skip(1)) // header: serial,fleet
{
    var columns = line.Split(',', 2, StringSplitOptions.TrimEntries);
    if (columns.Length < 2 || columns[0].Length == 0) continue;
    var (serial, fleetName) = (columns[0], columns[1]);

    if (!devices.TryGetValue(serial, out var device))
    {
        Console.WriteLine($"{serial}: no such device in your organisation");
        problems++;
        continue;
    }

    var matches = fleets[fleetName].ToList();
    if (matches.Count != 1)
    {
        Console.WriteLine($"{serial}: {(matches.Count == 0 ? "no fleet" : "more than one fleet")} named \"{fleetName}\"");
        problems++;
        continue;
    }

    var fleet = matches[0];
    if (device.FleetId == fleet.FleetId)
    {
        unchanged++;
        continue;
    }

    Console.WriteLine($"{serial}: {device.FleetName ?? "(no fleet)"} → {fleet.FleetName}");
    if (apply) await api.MoveDeviceAsync(device.DeviceId, new MoveDeviceRequest { FleetId = fleet.FleetId });
    moved++;
}

Console.WriteLine($"{(apply ? "Moved" : "Would move")} {moved}, already in place {unchanged}, problems {problems}." +
                  (apply || moved == 0 ? "" : " Run again with --apply to move them."));
