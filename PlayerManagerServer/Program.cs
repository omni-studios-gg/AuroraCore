using System.Net;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using PlayerManagerServer.Handlers;
using PlayerManagerServer.Handlers;
using PlayerManagerServer;
using PlayerManagerServer.Routers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<InventoryDB>();

var app = builder.Build();

app.MapWebSocketEndPoints();

app.Run("http://localhost:8080");