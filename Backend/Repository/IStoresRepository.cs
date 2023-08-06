using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;

namespace Backend.Repository;

public interface IStoresRepository
{
    Task<Store> GetById(int storeId);
    Task<(int totalCount, List<Store>)> GetByFilter(FilterModel filters);

    Task<List<StoreProduct>> GetProducts(int storeId, FilterModel filters);
    Task<Store> Add(Store store);
    Task<Store> Update(int storeId, StoreRequest request);
    Task<int> Count();
    Task<int> CountProducts(int storeId);
    Task<bool> Delete(List<int> ids);
}