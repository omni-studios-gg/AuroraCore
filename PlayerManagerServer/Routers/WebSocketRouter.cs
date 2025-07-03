using PlayerManagerServer.Handlers;

namespace PlayerManagerServer.Routers;

public static class WebSocketRouter
{
    public static void MapWebSocketEndPoints(this WebApplication app)
    {
        app.UseWebSockets();
        app.Map("/ws/inventory", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var db = context.RequestServices.GetRequiredService<InventoryDB>();
                await InventoryHandler.Handle(socket, db);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        });
    }
}