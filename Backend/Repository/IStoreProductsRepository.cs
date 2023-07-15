using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IStoreProductsRepository
{
    Task<StoreProduct> GetById(int storeProductId);
    Task<List<StoreProduct>> GetByFilter(FilterModel filters);
    Task<StoreProduct> Add(StoreProduct storeProduct);
    Task<StoreProduct> Update(int storeProductId, StoreProductRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}