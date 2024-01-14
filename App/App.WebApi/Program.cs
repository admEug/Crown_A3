using App.Application;
using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Contexts;
using App.Infrastructure.Shared;
using App.WebApi.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Services

    // Serilog configuration
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    Log.Information("Application startup services registration");

    // Projects' services
    builder.Services.AddApplicationLayer();
    builder.Services.AddPersistenceInfrastructure(builder.Configuration);
    builder.Services.AddSharedInfrastructure(builder.Configuration);

    // Controllers
    builder.Services.AddControllersExtension();

    // Swagger
    builder.Services.AddSwaggerExtension();

    // CORS
    builder.Services.AddCorsExtension();

    // Health Checks
    builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                                             tags: new[] { "database" })
    .AddDbContextCheck<ApplicationDbContext>(tags: new[] { "appDbContext" });
    builder.Services.AddHealthChecksUI().AddInMemoryStorage();

    //API Security
    builder.Services.AddJWTAuthentication(builder.Configuration);
    builder.Services.AddAuthorizationPolicies(builder.Configuration);

    // API version
    builder.Services.AddApiVersioningExtension();

    // API explorer
    builder.Services.AddMvcCore()
        .AddApiExplorer();

    // API explorer version
    builder.Services.AddVersionedApiExplorerExtension();

    #endregion

    var app = builder.Build();

    #region Middleware - Pipeline

    Log.Information("Application startup middleware registration");

    if (app.Environment.IsDevelopment())
    {
        // This will work without ErrorHandlerMiddleware being setup
        app.UseDeveloperExceptionPage();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
    else
    {
        // this works with the razor page Error.cshtml and without ErrorHandlerMiddleware being setup
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseRouting();

    //Enable CORS
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwaggerExtension();
    
    // Error - Exception Handler
    app.UseErrorHandlingMiddleware();

    // Health Checks
    app.MapHealthChecksUI();
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/cors", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).RequireCors("MyCorsPolicy");
    app.MapHealthChecks("/health/secure", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    })
    .RequireAuthorization();

    app.MapControllers();

    #endregion

    Log.Information("Application Starting");

    app.Run();
}
catch (Exception ex)
{
    Log.Warning(ex, "An error occurred starting the application");
}
finally
{
    Log.CloseAndFlush();
}