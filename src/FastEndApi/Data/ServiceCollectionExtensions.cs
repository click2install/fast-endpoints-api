using EntityFramework.Exceptions.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FastEndApi.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services)
    {
        services
            .AddTransient<AppDbContext>()
            .AddTransient<IAppDbContext, AppDbContext>()
            .AddTransient<IAppDbContextFactory, AppDbContextFactory>()
            .AddPooledDbContextFactory<AppDbContext>((svc, options) =>
            {
                var config = svc.GetRequiredService<IOptions<AppConfiguration>>();
                var datasource = AppDbContext.CreateDataSource(config.Value.ConnectionStrings.DefaultConnection);

                options
                    .UseNpgsql(datasource)
                    .UseLoggerFactory(svc.GetRequiredService<ILoggerFactory>())
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSnakeCaseNamingConvention()
                    .UseExceptionProcessor();
            });

        return services;
    }
}
