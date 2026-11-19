using System.Reflection;
using Lab13_GabrielNA.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab13_GabrielNA.Infrastructure.Configuration;

public static class DatabaseInitializer
{
    private const string Script = "Lab13_GabrielNA.Infrastructure.Scripts.linqexample.sql";

    public static async Task InicializarBaseDeDatosAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existe = await context.Database
            .SqlQueryRaw<bool>("SELECT to_regclass('public.\"Clients\"') IS NOT NULL AS \"Value\"")
            .SingleAsync();

        if (existe)
            return;

        await using var recurso = Assembly.GetExecutingAssembly().GetManifestResourceStream(Script)!;
        using var lector = new StreamReader(recurso);
        await context.Database.ExecuteSqlRawAsync(await lector.ReadToEndAsync());
    }
}
