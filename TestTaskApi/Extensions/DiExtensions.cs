using Microsoft.EntityFrameworkCore;
using TestTaskApi.Models.DbContext;

namespace TestTaskApi.Extensions;

public static class DiExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.UseMySQL(configuration.GetConnectionString("AuthSqlServerConnectionString") ??
                             throw new InvalidOperationException(
                                 "Incorrect connection string for MySql instance provided"),
                useMySqlBuilder =>
                {
                    useMySqlBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    useMySqlBuilder.EnableRetryOnFailure(40);
                });
        });

        return services;
    }
}