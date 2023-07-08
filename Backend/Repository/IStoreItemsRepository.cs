using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IStoreItemsRepository
{
    public Task<List<StoreItem>> GetByFilter(FilterModel filters);
    public Task<StoreItem> Add(StoreItem banner);
}