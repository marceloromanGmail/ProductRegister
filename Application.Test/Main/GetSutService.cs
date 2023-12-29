using Application._Common.Mapping;
using Application.Main.Products.Commands.CreateProduct;
using Application.Main.Products.Queries.GetProductById;
using Application.Services;
using Application.ServicesClients.DiscountsProduct;
using Application.Test.Context;
using AutoMapper;
using Moq;

namespace Application.Test.Main
{
    public interface IGetSutService<T>
    {
        T GetSutService();
    }

    public abstract class GetSutService : MockedMainContext
    {
        protected Mapper Mapper { get; }

        protected GetSutService()
        {
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                var instanceMap = new MappingProfile(typeof(DependencyInjection).Assembly);
                cfg.AddProfile(instanceMap);
            }));
        }
    }

    public class GetCreateProductCommandHandlerSutService : GetSutService, IGetSutService<CreateProductCommandHandler>
    {
        private Mock<IProductStatusService> _productStatusServiceMock;

        public GetCreateProductCommandHandlerSutService(Mock<IProductStatusService> productStatusServiceMock)
        {
            _productStatusServiceMock = productStatusServiceMock;
        }

        public CreateProductCommandHandler GetSutService()
        {
            return new CreateProductCommandHandler(Context, Mapper, _productStatusServiceMock.Object);
        }
    }

    public class GetProductByIdQueryHandlerSutService : GetSutService, IGetSutService<GetProductByIdQueryHandler>
    {
        private Mock<IProductStatusService> _productStatusServiceMock;
        private Mock<IDiscountsProductServiceClient> _discountsProductServiceClientMock;

        public GetProductByIdQueryHandlerSutService(
            Mock<IProductStatusService> productStatusServiceMock,
            Mock<IDiscountsProductServiceClient> discountsProductServiceClientMock)
        {
            _productStatusServiceMock = productStatusServiceMock;
            _discountsProductServiceClientMock = discountsProductServiceClientMock;
        }

        public GetProductByIdQueryHandler GetSutService()
        {
            return new GetProductByIdQueryHandler(Context, Mapper, _productStatusServiceMock.Object, _discountsProductServiceClientMock.Object);
        }
    }

    public class UpdateProductCommandHandlerSutService : GetSutService, IGetSutService<UpdateProductCommandHandler>
    {
        public UpdateProductCommandHandler GetSutService()
        {
            return new UpdateProductCommandHandler(Context, Mapper);
        }
    }
}