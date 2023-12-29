using Application.Main.Products.Commands.CreateProduct;
using Application.Services;
using Domain.Main.Enums;
using Moq;

namespace Application.Test.Main.Commands
{
    public class CreateProductCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnCreatedProductDto()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = Guid.NewGuid().ToString(),
                Status = (byte)ProductStatus.Active,
                Stock = 1,
                Description = Guid.NewGuid().ToString(),
                Price = 1
            };
            var productServiceMock = new Mock<IProductStatusService>();
            productServiceMock
                .Setup(p => p.GetProductStatusAsync())
                .ReturnsAsync(new Dictionary<byte, string> { { (byte)ProductStatus.Active, ProductStatus.Active.ToString() } });
            var sut = new GetCreateProductCommandHandlerSutService(productServiceMock).GetSutService();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(ProductStatus.Active.ToString(), result.StatusName);
        }
    }
}
