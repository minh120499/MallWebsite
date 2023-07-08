using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class StoreItemsService
{
    private readonly IStoreItemsRepository _storeItemsRepository;

    public StoreItemsService(IStoreItemsRepository storeItemsRepository)
    {
        _storeItemsRepository = storeItemsRepository;
    }

    public async Task<List<StoreItem>> GetByFilter(FilterModel filters)
    {
        var storeItems = await _storeItemsRepository.GetByFilter(filters);
        return storeItems;
    }

    public async Task<StoreItem> Create(StoreItemRequest request)
    {
        Validations.StoreItem(request);

        var storeItem = new StoreItem()
        {
            Name = request.Name,
            Available = request.Available,
            StoreId = request.StoreId,
            Price = request.Price,
            Status = StatusConstraint.ACTIVE,
        };
        return await _storeItemsRepository.Add(storeItem);
    }
}