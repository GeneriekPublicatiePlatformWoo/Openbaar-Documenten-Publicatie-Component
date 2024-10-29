using Microsoft.EntityFrameworkCore;
using ODPC.Apis.Odrc;
using ODPC.Authentication;
using ODPC.Data;
using ODPC.Features;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

using var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information) //logeventlevel information voor Microsoft.AspNetCore.Authentication namespace omdat deze namespace de unauthorizations gooit, voorbeeld: Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler: Information: AuthenticationScheme: Bearer was challenged.
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

logger.Information("Starting up");

try
{
    builder.Host.UseSerilog(logger);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();

    string GetRequiredConfig(string key)
    {
        var value = builder.Configuration[key];
        return string.IsNullOrWhiteSpace(value)
            ? throw new Exception($"Environment variable {key} is missing or empty")
            : value;
    };

    builder.Services.AddAuth(options =>
    {
        options.Authority = GetRequiredConfig("OIDC_AUTHORITY");
        options.ClientId = GetRequiredConfig("OIDC_CLIENT_ID");
        options.ClientSecret = GetRequiredConfig("OIDC_CLIENT_SECRET");
        options.AdminRole = GetRequiredConfig("OIDC_ADMIN_ROLE");
        options.NameClaimType = builder.Configuration["OIDC_NAME_CLAIM_TYPE"];
        options.RoleClaimType = builder.Configuration["OIDC_ROLE_CLAIM_TYPE"];
        options.IdClaimType = builder.Configuration["OIDC_ID_CLAIM_TYPE"];
    });

    var connStr = $"Username={builder.Configuration["POSTGRES_USER"]};Password={builder.Configuration["POSTGRES_PASSWORD"]};Host={builder.Configuration["POSTGRES_HOST"]};Database={builder.Configuration["POSTGRES_DB"]};Port={builder.Configuration["POSTGRES_PORT"]}";
    builder.Services.AddDbContext<OdpcDbContext>(opt => opt.UseNpgsql(connStr));
    builder.Services.AddScoped<IOdrcClientFactory, OdrcClientFactory>();
    builder.Services.AddScoped<IGebruikerWaardelijstItemsService, GebruikerWaardelijstItemsService>();


    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseDefaultFiles();
    app.UseOdpcStaticFiles();
    app.UseOdpcSecurityHeaders();

    app.UseAuthorization();

    app.MapControllers().RequireAuthorization();

    app.MapOdpcAuthEndpoints();
    app.MapHealthChecks("/healthz").AllowAnonymous();
    app.MapFallbackToIndexHtml();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        await scope.ServiceProvider.GetRequiredService<OdpcDbContext>().Database.MigrateAsync();
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    logger.Write(LogEventLevel.Fatal, ex, "Application terminated unexpectedly");
}

public partial class Program { }
