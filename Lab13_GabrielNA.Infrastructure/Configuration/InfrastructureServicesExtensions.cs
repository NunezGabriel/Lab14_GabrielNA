using Lab13_GabrielNA.Application.Ports.Reports;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Infrastructure.Adapters.Reports;
using Lab13_GabrielNA.Infrastructure.Adapters.Repositories;
using Lab13_GabrielNA.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lab13_GabrielNA.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = NormalizarConnectionString(configuration.GetConnectionString("DefaultConnection")!);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExcelReportGenerator, ClosedXmlReportGenerator>();

        return services;
    }

    private static string NormalizarConnectionString(string connectionString)
    {
        if (!connectionString.StartsWith("postgres://") && !connectionString.StartsWith("postgresql://"))
            return connectionString;

        var uri = new Uri(connectionString);
        var credenciales = uri.UserInfo.Split(':', 2);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host     = uri.Host,
            Port     = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(credenciales[0]),
            Password = credenciales.Length > 1 ? Uri.UnescapeDataString(credenciales[1]) : string.Empty
        };

        return builder.ConnectionString;
    }
}
