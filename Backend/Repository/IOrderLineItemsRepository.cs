using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IOrderLineItemsRepository
{
    Task<OrderLineItem> GetById(int orderLineItemId);
    Task<List<OrderLineItem>> GetByFilter(FilterModel filters);
    Task<OrderLineItem> Add(OrderLineItem orderLineItem);
    Task<OrderLineItem> Update(int orderLineItemId, OrderLineItemRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}