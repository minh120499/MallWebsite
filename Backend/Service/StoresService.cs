using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class StoresService
{
    private readonly IStoresRepository _storesRepository;

    public StoresService(IStoresRepository storesRepository)
    {
        _storesRepository = storesRepository;
    }

    public async Task<List<Store>> GetByFilter(FilterModel filters)
    {
        var stores = await _storesRepository.GetByFilter(filters);
        return stores;
    }

    public async Task<Store> Create(StoreRequest request)
    {
        Validations.Store(request);

        var store = new Store()
        {
            Name = request.Name,
            Status = StatusConstraint.ACTIVE,
        };
        return await _storesRepository.Add(store);
    }
}