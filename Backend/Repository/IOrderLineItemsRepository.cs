using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IOrderLineItemsRepository
{
    public Task<List<OrderLineItem>> GetByFilter(FilterModel filters);
    public Task<OrderLineItem> Add(OrderLineItem banner);
    public Task<int> Count();
}