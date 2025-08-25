using PackageManage.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.BuildModule();

builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<CurrentUserMiddleware>();

app.Run();
