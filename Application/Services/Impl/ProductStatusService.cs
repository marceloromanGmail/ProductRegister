using Application.Components;
using Domain.Main.Enums;

namespace Application.Services.Impl
{
    internal class ProductStatusService : IProductStatusService
    {
        private readonly IDataCacheService _cache;
        private const string KeyProductSatus = "status-product";
        public ProductStatusService(IDataCacheService cache)
        {
            _cache = cache;
        }

        public async Task<Dictionary<byte, string>> GetProductStatusAsync()
        {
            var data = await _cache.Get<Dictionary<byte, string>>(KeyProductSatus);
            if (data != null) return data;

            data = new Dictionary<byte, string>
            {
                { (byte)ProductStatus.Inactive, ProductStatus.Inactive.ToString()},
                { (byte)ProductStatus.Active, ProductStatus.Active.ToString()},
            };

            await _cache.Put(KeyProductSatus, data, TimeSpan.FromMinutes(5));
            return data;
        }
    }
}
