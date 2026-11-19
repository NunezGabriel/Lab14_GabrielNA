using Lab13_GabrielNA.Configuration;
using Lab13_GabrielNA.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.Services.InicializarBaseDeDatosAsync();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reportes Excel API v1");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
