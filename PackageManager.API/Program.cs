using PackageManage.API.Middleware;
using PackageManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddMediatR();
builder.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<CurrentUserMiddleware>();

app.Run();
