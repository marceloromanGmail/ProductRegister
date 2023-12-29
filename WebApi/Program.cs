using Application;
using DataAccess;
using Infraestructure.Components.Cache;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using WebApi;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

builder.Logging.AddSerilog(logger);

var environment = Environment.GetEnvironmentVariable(ApiConfiguration.GLOBAL_ENV_VAR);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddPresentation(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCahceInMemory(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomErrorHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Globalization and Localization Support
var apiConfiguration = app.Services.GetRequiredService<ApiConfiguration>();
var languagesSuported = new[] { "es-ES", "en-US" };
var localizaoptions = new RequestLocalizationOptions()
    .SetDefaultCulture(apiConfiguration.DefaultCulture)
    .AddSupportedCultures(languagesSuported)
    .AddSupportedUICultures(languagesSuported);

localizaoptions.RequestCultureProviders.Clear();
localizaoptions.RequestCultureProviders.Add(new WebApi.AppStart.CustomRequestCultureProvider(apiConfiguration));
app.UseRequestLocalization(localizaoptions);

app.Run();
