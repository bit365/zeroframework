var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startup = new ZeroFramework.DeviceCenter.API.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

startup.Configure(app, app.Environment);

app.Run();