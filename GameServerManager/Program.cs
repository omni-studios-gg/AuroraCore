var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
app.UseCors();