using Application.ServicesClients.DiscountsProduct;
using Application.ServicesClients.DiscountsProduct.Models.Output;

namespace Infraestructure.ServicesClients.DiscountsProduct
{
    public class DiscountsProductFakeServiceClient : IDiscountsProductServiceClient
    {
        public Task<DiscountProductDto> GetDiscountByProduct(int productId)
        {
            var priceProductDto = new DiscountProductDto
            {
                Id = productId,
                Name = Guid.NewGuid().ToString(),
                Discount = new Random(50).NextDouble()
            };
            return Task.FromResult(priceProductDto);
        }
    }
}
