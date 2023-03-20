using Microsoft.EntityFrameworkCore;
using TestTaskApi.Common.MappingProfiles;
using TestTaskApi.Interfaces.Repositories;
using TestTaskApi.Interfaces.Services;
using TestTaskApi.Models.DbContext;
using TestTaskApi.Repositories;
using TestTaskApi.Services;

namespace TestTaskApi.Extensions;

public static class DiExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IJobInformationRepository, JobInformationRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IJobInformationService, JobInformationService>();

        services.AddAutoMapper(expression => { expression.AddProfile<ApplicationMappingProfile>(); });

        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.UseMySQL(configuration.GetConnectionString("MySqlConnectionString") ??
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