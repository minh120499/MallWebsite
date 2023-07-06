using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IStoresRepository
{
    public Task<List<Store>> GetByFilter(FilterModel filters);
    public Task<Store> Add(Store banner);
}