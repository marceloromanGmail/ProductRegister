using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    //[Fact]
    //public async Task Post_ReturnsBadRequestObjectResult_WithValidationErrors()
    //{
    //    // Arrange
    //    var mediatorMock = new Mock<IMediator>();
    //    var sut = new ProductsController(mediatorMock.Object);

    //    var createProductCommand = new CreateProductCommand
    //    {
    //        Name = "",
    //        Description = "",
    //        Stock = -1,
    //        Price = 0,
    //        Status = 2
    //    };

    //    var validator = new CreateProductCommandValidator();
    //    var validationResults = validator.Validate(createProductCommand);

    //    var mediatorResult = new Mock<IRequestHandler<CreateProductCommand, CreatedProductDto>>();
    //    mediatorResult.Setup(m => m.Handle(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(new CreatedProductDto()); // You can return a valid result here since validation is already being tested separately

    //    // Act
    //    var result = await sut.Post(createProductCommand);

    //    // Assert
    //    var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    //    //var validationResult = Assert.IsType<ValidationResult>(badRequestObjectResult.Value);

    //    //Assert.Equal(validationResults.Errors.Count, validationResult.Errors.Count);

    //    // Assert specific error messages based on your validation rules
    //    //foreach (var error in validationResults.Errors)
    //    //{
    //    //    Assert.Contains(validationResult.Errors, e => e.ErrorMessage == error.ErrorMessage && e.PropertyName == error.PropertyName);
    //    //}
    //}

    //[Fact]
    //public async Task Post_InvalidModel_ReturnsBadRequest()
    //{
    //    // Arrange
    //    var client = _factory.CreateClient();
    //    var invalidProduct = new CreateProductCommand(); // An intentionally invalid object

    //    // Act
    //    var response = await client.PostAsJsonAsync("/api/products", invalidProduct);

    //    // Assert
    //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //}
}
