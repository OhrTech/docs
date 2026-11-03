#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=false
#:package OhrTech.Management@*
// A minimal help desk: a list of your devices with a Connect button that opens the device's
// screen right in the page.
//   OHR_TOKEN=ohrs_… dotnet run help-desk-connect.cs     then open http://localhost:5000
// Needs ViewDevices and RemoteControl.
using OhrTech.Management;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
builder.Services.AddSingleton(_ => OhrManagementClient.ForServiceAccount(
    builder.Configuration["OHR_TOKEN"] ?? throw new InvalidOperationException("Set OHR_TOKEN to a service account token.")));
var app = builder.Build();

app.MapGet("/", () => Results.Content(HelpDesk.Page, "text/html"));

// The devices your agents can pick from.
app.MapGet("/devices", async (OhrManagementClient api) =>
    (await api.ListDevicesAsync()).Select(d => new { d.DeviceId, d.Serial, d.FleetName, d.Online, d.RemoteControl }));

// Your server opens the session; the browser only ever sees the short-lived embed link.
// In a real help desk, check here that the signed-in agent may support this device.
app.MapPost("/devices/{deviceId:long}/connect", async (long deviceId, HttpRequest request, OhrManagementClient api) =>
{
    var thisSite = $"{request.Scheme}://{request.Host}";
    try
    {
        var rc = await api.OpenRemoteControlSessionAsync(deviceId, new OpenRemoteControlRequest
        {
            EmbedOrigins = [thisSite], // only this site may show the viewer
        });
        return Results.Ok(new { rc.Session.SessionId, rc.Embed.Url });
    }
    catch (OhrApiException<ProblemDetails> e)
    {
        // 409: someone else is connected (retry with TakeOver = true to end their session).
        return Results.Json(new { e.Result.Title, e.Result.Detail }, statusCode: e.StatusCode);
    }
});

app.MapDelete("/sessions/{sessionId}", async (Guid sessionId, OhrManagementClient api) =>
{
    await api.CloseRemoteControlSessionAsync(sessionId);
    return Results.NoContent();
});

app.Run();

static class HelpDesk
{
    public const string Page = """
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Help desk</title>
  <style>
    body { font: 16px system-ui, sans-serif; margin: 0; display: grid; grid-template-columns: 22rem 1fr; height: 100vh; }
    aside { border-right: 1px solid #dde1e8; overflow: auto; padding: 1rem; }
    li { display: flex; justify-content: space-between; align-items: center; padding: .5rem 0; border-bottom: 1px solid #eef0f3; }
    main { display: grid; grid-template-rows: auto 1fr; }
    #status { padding: .75rem 1rem; border-bottom: 1px solid #dde1e8; }
    iframe { width: 100%; height: 100%; border: 0; background: #111; }
  </style>
</head>
<body>
  <aside><h2>Devices</h2><ul id="devices" style="list-style:none;padding:0"></ul></aside>
  <main>
    <div id="status">Pick a device.</div>
    <iframe id="viewer" allow="fullscreen; clipboard-read" title="Device screen"></iframe>
  </main>
  <script>
    const list = document.getElementById("devices"), viewer = document.getElementById("viewer");
    const status = document.getElementById("status");

    fetch("/devices").then(r => r.json()).then(devices => {
      for (const d of devices) {
        const li = document.createElement("li");
        li.textContent = `${d.serial ?? d.deviceId} · ${d.fleetName ?? "no fleet"}`;
        const button = document.createElement("button");
        button.textContent = "Connect";
        button.disabled = !d.online || !d.remoteControl;
        button.onclick = () => connect(d.deviceId);
        li.append(button);
        list.append(li);
      }
    });

    async function connect(deviceId) {
      status.textContent = "Connecting…";
      const r = await fetch(`/devices/${deviceId}/connect`, { method: "POST" });
      const body = await r.json();
      if (!r.ok) { status.textContent = body.detail ?? body.title; return; }
      viewer.src = body.url;
    }

    // The viewer reports what is happening on the device.
    window.addEventListener("message", e => {
      if (e.origin !== "https://api.ohrtech.net" || e.data?.source !== "ohrtech-remote-control") return;
      if (e.data.type === "state") {
        status.textContent = {
          waiting: "Waiting for the device…",
          awaiting_consent: "Waiting for the person at the device to allow it…",
          active: "Connected.",
          ended: "Session ended.",
          error: "Could not connect.",
        }[e.data.state] ?? e.data.state;
      }
    });
  </script>
</body>
</html>
""";
}
