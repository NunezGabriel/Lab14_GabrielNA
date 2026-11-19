using Lab13_GabrielNA.Application;
using Lab13_GabrielNA.Infrastructure.Configuration;
using Microsoft.OpenApi;

namespace Lab13_GabrielNA.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationAssemblyMarker).Assembly
        ));

        services.AddInfrastructureServices(configuration);

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "Reportes Excel API",
                Version     = "v1",
                Description = "API de reportes en Excel (ClosedXML) sobre la base de datos linqexample, con arquitectura hexagonal, patrón repositorio y CQRS (MediatR). Lab 14: desplegada en Render con CI/CD desde GitHub Actions."
            });
        });

        return services;
    }
}
