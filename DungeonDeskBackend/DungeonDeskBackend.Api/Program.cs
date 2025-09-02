using DungeonDeskBackend.Api.Extensions;
using DungeonDeskBackend.Application.Extensions;
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApiServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);
var app = builder.Build();
app.UseApiServices();
app.Run();
