using Application._Common.Mapping;
using Application.Components;
using Application.ServicesClients.DiscountsProduct;
using Infraestructure;
using Infraestructure.Components.WriteInFile;
using Infraestructure.ServicesClients.DiscountsProduct;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(expression =>
            {
                expression.AddProfile(new MappingProfile(typeof(DependencyInjection).Assembly));
            });

            var discountProductBaseUrl = configuration["ServicesClients:DiscountProduct:Url"];
            var priceProductFake = bool.Parse(configuration["ServicesClients:PricesProduct:Fake"] ?? "false");
            var logRequestPathFile = configuration["AppSettings:RequestLogPathFile"];
            services.AddHttpClient(InfraestructureConfiguration.DiscountProductConfigHttp, (http) =>
            {
                http.BaseAddress = new Uri(discountProductBaseUrl);
            });

            services.AddScoped<IDiscountsProductServiceClient, DiscountsProductServiceClient>();
            services.AddTransient<IWriteLineInFileService, WriteLineInFileService>(provider =>
            {
                return new WriteLineInFileService(logRequestPathFile);
            });

            if (priceProductFake) services.AddScoped<IDiscountsProductServiceClient, DiscountsProductFakeServiceClient>();

            return services;
        }
    }
}
