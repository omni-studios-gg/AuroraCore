using Authserver.DBContexts;
using AuroraCore;
using Authserver.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

// Suppress Info/Debug noise

// ✅ Register Services
builder.Services.AddControllers();          // Enables API controllers
builder.Services.AddOpenApi();             // Enables Swagger/OpenAPI
builder.Services.AddScoped<AuthenticationHelper>();
builder.Services.AddCors(options => {options.AddDefaultPolicy(policy => {policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    new MySqlServerVersion(new Version(8, 0, 21))));

var app = builder.Build();

// ✅ Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi(); // Swagger in dev
}
else
{
    // Optional: enable Swagger in production too
    app.MapOpenApi(); // Comment this if you want to hide it in prod
}

// ✅ Register Endpoints
app.UseRouting();
app.MapControllers();        // Maps [ApiController] routes
app.MapAuthEndpoints();      // Your custom endpoints
app.MapGet("/", () => "✅ Authserver is running!");

// ✅ Start App
app.UseCors();
app.Run();