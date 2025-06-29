using Microsoft.AspNetCore.Mvc;
using AuroraCore;

namespace Authserver.Endpoints;

public static class AuthenticationEndpoint
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/authtoken", ([FromServices]AuthenticationHelper authHelper) =>
        {
            int token = authHelper.GetAuthToken();
            return Results.Ok(new { token });
        });
    }
}