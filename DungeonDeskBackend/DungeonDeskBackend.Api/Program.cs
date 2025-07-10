using DungeonDeskBackend.Api.Extensions;
using DungeonDeskBackend.Application.Extensions;
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApiServices();
builder.Services.ConfigureApplicationServices();
var app = builder.Build();
app.UseApiServices();
app.Run();
