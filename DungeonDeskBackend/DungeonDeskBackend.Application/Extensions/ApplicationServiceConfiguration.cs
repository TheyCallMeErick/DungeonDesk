

using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ObservatorioApi.Auth;

namespace DungeonDeskBackend.Application.Extensions;

public static class ApplicationServiceConfiguration
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDeskService, DeskService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<ITokenManagerService, TokenManagerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddDbContext<DungeonDeskDbContext>();
    }
}
