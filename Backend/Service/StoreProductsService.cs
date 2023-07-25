using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class StoreProductsService
    {
        private readonly IStoreProductsRepository _storeProductsRepository;

        public StoreProductsService(IStoreProductsRepository storeProductsRepository)
        {
            _storeProductsRepository = storeProductsRepository;
        }

        public async Task<StoreProduct> GetById(int storeProductId)
        {
            return await _storeProductsRepository.GetById(storeProductId);
        }

        public async Task<TableListResponse<StoreProduct>> GetByFilter(FilterModel filters)
        {
            var storeProducts = await _storeProductsRepository.GetByFilter(filters);
            var total = await _storeProductsRepository.Count();
            return new TableListResponse<StoreProduct>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = storeProducts
            };
        }

        public async Task<StoreProduct> Create(StoreProductRequest request)
        {
            Validations.StoreProduct(request);

            var storeProduct = new StoreProduct()
            {
                StoreId = request.StoreId,
                ProductId = request.ProductId,
                // Quantity = request.Quantity,
            };
            return await _storeProductsRepository.Add(storeProduct);
        }

        public async Task<StoreProduct> Update(int storeProductId, StoreProductRequest request)
        {
            Validations.StoreProduct(request);

            return await _storeProductsRepository.Update(storeProductId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var storeProductIds = ids.Split(',').Select(int.Parse).ToList();

            return await _storeProductsRepository.Delete(storeProductIds);
        }
    }
}
