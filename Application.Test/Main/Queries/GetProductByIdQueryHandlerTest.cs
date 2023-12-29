using Application._Common.Exceptions;
using Application.Main.Products.Queries.GetProductById;
using Application.Services;
using Application.ServicesClients.DiscountsProduct;
using Application.ServicesClients.DiscountsProduct.Models.Output;
using Domain.Main.Entities;
using Domain.Main.Enums;
using Moq;

namespace Application.Test.Main.Queries
{
    public class GetProductByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnDetailProductDto()
        {
            // Arrange
            var productServiceMock = new Mock<IProductStatusService>();
            productServiceMock
                .Setup(p => p.GetProductStatusAsync())
                .ReturnsAsync(new Dictionary<byte, string> { { (byte)ProductStatus.Active, ProductStatus.Active.ToString() } });
            var discountsProductServiceClientMock = new Mock<IDiscountsProductServiceClient>();
            var discountProductDto = new DiscountProductDto
            {
                Id = 1,
                Discount = 10,
                Name = Guid.NewGuid().ToString()
            };
            discountsProductServiceClientMock
                .Setup(p => p.GetDiscountByProduct(It.IsAny<int>()))
                .ReturnsAsync(discountProductDto);
            var sutService = new GetProductByIdQueryHandlerSutService(productServiceMock, discountsProductServiceClientMock);
            var product = new Product
            {
                Name = Guid.NewGuid().ToString(),
                Status = (byte)ProductStatus.Active,
                Stock = 1,
                Description = Guid.NewGuid().ToString(),
                Price = 100
            };
            sutService.Context.Add(product);
            await sutService.Context.SaveChangesAsync();
            var sut = sutService.GetSutService();

            // Act
            var result = await sut.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Stock, result.Stock);
            Assert.Equal(product.Status, result.Status);
            Assert.Equal(ProductStatus.Active.ToString(), result.StatusName);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(discountProductDto.Discount, result.Discount);
            Assert.Equal(product.Price * (100 - discountProductDto.Discount) / 100, result.FinalPrice);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ShouldNotReturnDetailProductDtoAndThrowException()
        {
            // Arrange
            var productServiceMock = new Mock<IProductStatusService>();
            var discountsProductServiceClientMock = new Mock<IDiscountsProductServiceClient>();
            var sutService = new GetProductByIdQueryHandlerSutService(productServiceMock, discountsProductServiceClientMock);
            var product = new Product
            {
                Name = Guid.NewGuid().ToString(),
                Status = (byte)ProductStatus.Active,
                Stock = 1,
                Description = Guid.NewGuid().ToString(),
                Price = 100
            };
            sutService.Context.Add(product);
            await sutService.Context.SaveChangesAsync();
            var sut = sutService.GetSutService();

            // Act and Assert
            _ = Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(new GetProductByIdQuery(product.Id + 1), CancellationToken.None));
        }
    }
}