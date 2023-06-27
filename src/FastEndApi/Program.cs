global using FastEndpoints;


using System.Reflection;
using System.Text.Json;

using FastEndApi;
using FastEndApi.Data;
using FastEndApi.Data.Models;
using FastEndApi.Extensions;
using FastEndApi.Features.Authorization.ApiKey;
using FastEndApi.Swagger;

using FastEndpoints.Swagger;

using FluentValidation;

using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
        options.AllowSynchronousIO = false;
    });

builder.Host
    .UseConsoleLifetime(options => options.SuppressStatusMessages = true);

var config = builder.Configuration.Get<AppConfiguration>()!;

builder.Services
    .AddSingleton<IValidator<AppConfiguration>, AppConfigurationValidator>()
    .AddOptions<AppConfiguration>()
    .Bind(builder.Configuration)
    .ValidateFluent()
    .ValidateOnStart()
    .Services

    .AddDatabaseContext()

    .AddTransient<IPasswordHasher<User>, PasswordHasher<User>>()
    .AddFeatures(Assembly.GetExecutingAssembly())

    .AddHealthChecks()
    .AddApplicationStatus()
    .AddNpgSql(config.ConnectionStrings.DefaultConnection)
    .Services

    .AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = Array.Empty<Type>())

    .AddAuthentication(ApiKeyAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationDefaults.AuthenticationScheme, null)
    .Services

    .SwaggerDocument(options =>
    {
        options.EnableJWTBearerAuth = false;
        options.ShortSchemaNames = true;
        options.TagDescriptions = dict => SwaggerTagCollector.Collect(dict, Assembly.GetExecutingAssembly());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
}

app.MapHealthChecks("/api/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    })
    .AllowAnonymous();

app.UseAuthorization()

    .UseFastEndpoints(options =>
    {
        options.Endpoints.RoutePrefix = "api";
        options.Errors.UseProblemDetails();
        options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })

    .UseSwaggerUi3(options =>
    {
        options.ConfigureDefaults();

        options.AdditionalSettings["filter"] = false;
        options.DocExpansion = "list";
        options.DefaultModelsExpandDepth = -1;
    })
    .UseOpenApi();

await app.RunAsync();

public partial class Program { }