using Application._Common.Exceptions;
using Application.ServicesClients.DiscountsProduct;
using Application.ServicesClients.DiscountsProduct.Models.Output;
using AutoMapper;
using Infraestructure.ServicesClients.DiscountsProduct.Models;
using System.Net.Http.Json;

namespace Infraestructure.ServicesClients.DiscountsProduct
{
    public class DiscountsProductServiceClient : IDiscountsProductServiceClient
    {
        private const string GetPriceProduct = "api/discount/{id}";

        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public DiscountsProductServiceClient(IHttpClientFactory factory, IMapper mapper)
        {
            _httpClient = factory.CreateClient(InfraestructureConfiguration.DiscountProductConfigHttp);
            _mapper = mapper;
        }

        public async Task<DiscountProductDto> GetDiscountByProduct(int productId)
        {
            var url = GetPriceProduct.Replace("{id}", productId.ToString());
            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url, UriKind.Relative)
            });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DiscountsProductApiDto>();
                return _mapper.Map<DiscountProductDto>(result);
            }

            throw new BusinessException("It has not been possible to recover the price of the product");
        }
    }
}
