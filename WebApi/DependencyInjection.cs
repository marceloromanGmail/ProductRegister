using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ApiConfiguration>();

            services.AddMemoryCache();
            services.AddControllers();
            services.AddOptions();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization()
                     .AddJsonOptions(options =>
                     {
                         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                         options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                     });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            return services;
        }
    }
}