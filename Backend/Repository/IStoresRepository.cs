using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IStoresRepository
{
    Task<Store> GetById(int storeId);
    Task<List<Store>> GetByFilter(FilterModel filters);
    Task<Store> Add(Store store);
    Task<Store> Update(int storeId, StoreRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}