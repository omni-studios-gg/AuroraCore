using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlayerManagerServer.Handlers;

public static class InventoryHandler
{
    public static async Task Handle(WebSocket socket, InventoryDB db)
    {
        var buffer = new byte[4096];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine("📥 Received: " + json);

            var data = JObject.Parse(json);
            string action = data["action"]?.ToString();
            int playerId = data["playerId"]?.ToObject<int>() ?? 0;

            if (action == "request_inventory")
            {
                var items = db.LoadInventory(playerId);
                var response = JsonConvert.SerializeObject(new { Items = items });
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await socket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine("📤 Sent inventory to playerId " + playerId);
            }
        }
    }
}