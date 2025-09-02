

using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Repositories;
using DungeonDeskBackend.Application.Repositories.Interfaces;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using ObservatorioApi.Auth;

namespace DungeonDeskBackend.Application.Extensions;

public static class ApplicationServiceConfiguration
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDeskService, DeskService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IDeskRepository, DeskRepository>();
        services.AddScoped<ITokenManagerService, TokenManagerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddDbContext<DungeonDeskDbContext>(x =>
        {
            x.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptionsAction: npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });

            if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                x.UseSeeding((context,_) =>
                {
                    Seeder.Init();
                    context.Set<User>().AddRange(Seeder.Users);
                    context.Set<Player>().AddRange(Seeder.Players);
                    context.Set<Adventure>().AddRange(Seeder.Adventures);
                    context.Set<Chronicle>().AddRange(Seeder.Chronicles);
                    context.Set<Desk>().AddRange(Seeder.Desks);
                    context.Set<PlayerDesk>().AddRange(Seeder.PlayerDesks);
                    context.Set<Session>().AddRange(Seeder.Sessions);
                    context.SaveChanges();
                });
            }
        });
    }
}
