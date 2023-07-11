using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class StoreProductsService
{
    private readonly IStoreProductsRepository _storeProductsRepository;

    public StoreProductsService(IStoreProductsRepository storeProductsRepository)
    {
        _storeProductsRepository = storeProductsRepository;
    }

    public async Task<List<StoreProduct>> GetByFilter(FilterModel filters)
    {
        var storeProducts = await _storeProductsRepository.GetByFilter(filters);
        return storeProducts;
    }

    public async Task<StoreProduct> Create(StoreProductRequest request)
    {
        Validations.StoreProduct(request);

        var storeProduct = new StoreProduct()
        {
            // Name = request.Name,
            // Available = request.Available,
            StoreId = request.StoreId,
            Price = request.Price,
            Status = StatusConstraint.ACTIVE,
        };
        return await _storeProductsRepository.Add(storeProduct);
    }
}