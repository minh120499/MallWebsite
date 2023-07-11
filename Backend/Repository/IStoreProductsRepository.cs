using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IStoreProductsRepository
{
    public Task<List<StoreProduct>> GetByFilter(FilterModel filters);
    public Task<StoreProduct> Add(StoreProduct banner);
}