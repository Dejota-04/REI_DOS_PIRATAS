using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Sprint1.Data;
using Sprint1.TagHelpers;
using System.Globalization;
using Serilog;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day) // Garante o  em arquivo exigido
    .CreateLogger();

try
{
    Log.Information("Iniciando a API...");
    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog();

    builder.Services.AddOpenTelemetry()
        .WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter();
        })
        .WithMetrics(metricsProviderBuilder =>
        {
            metricsProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        });


    builder.Services.AddHealthChecks()
        .AddCheck("API", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API rodando liso"))
        .AddDbContextCheck<ApplicationDbContext>("Banco_Oracle");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

    builder.Services.AddLocalization();
    builder.Services.AddControllersWithViews();

    var supportedCultures = new[] { new CultureInfo("pt-BR") };
    var localizationOptions = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("pt-BR"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    };

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseRequestLocalization(localizationOptions);

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new { componente = e.Key, status = e.Value.Status.ToString() })
            });
            await context.Response.WriteAsync(result);
        }
    });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou catastroficamente durante a inicialização");
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program { }