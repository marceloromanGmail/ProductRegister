using Application._Common.Exceptions;
using Application.Main.Products.Commands.CreateProduct;
using Domain.Main.Entities;
using Domain.Main.Enums;

namespace Application.Test.Main.Commands
{
    public class UpdateProductCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateProduct()
        {
            // Arrange
            var sutService = new UpdateProductCommandHandlerSutService();
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
            var command = new UpdateProductCommand
            {
                Id = product.Id,
                Name = Guid.NewGuid().ToString(),
                Status = (byte)ProductStatus.Active,
                Stock = 2,
                Description = Guid.NewGuid().ToString(),
                Price = 50
            };
            var sut = sutService.GetSutService();

            // Act
            await sut.Handle(command, CancellationToken.None);

            // Assert
            var actual = sutService.Context.Products.First(p => p.Id == product.Id);
            Assert.Equal(command.Name, actual.Name);
            Assert.Equal(command.Description, actual.Description);
            Assert.Equal(command.Price, actual.Price);
            Assert.Equal(command.Stock, actual.Stock);
            Assert.Equal(command.Status, actual.Status);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldNotUpdateProductAndThrowException()
        {
            // Arrange
            var sutService = new UpdateProductCommandHandlerSutService();
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
            var command = new UpdateProductCommand
            {
                Id = product.Id + 1,
                Name = Guid.NewGuid().ToString(),
                Status = (byte)ProductStatus.Active,
                Stock = 2,
                Description = Guid.NewGuid().ToString(),
                Price = 50
            };
            var sut = sutService.GetSutService();

            // Act and Assert
            _ = Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
        }
    }
}