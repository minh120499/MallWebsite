using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class OrderLineItemsService
{
    private readonly IOrderLineItemsRepository _orderLineItemsRepository;

    public OrderLineItemsService(IOrderLineItemsRepository orderLineItemsRepository)
    {
        _orderLineItemsRepository = orderLineItemsRepository;
    }

    public async Task<TableListResponse<OrderLineItem>> GetByFilter(FilterModel filters)
    {
        var orderLineItems = await _orderLineItemsRepository.GetByFilter(filters);
        var total = await _orderLineItemsRepository.Count();
        return new TableListResponse<OrderLineItem>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = orderLineItems
        };
    }

    public async Task<OrderLineItem> Create(OrderLineItemRequest request)
    {
        Validations.OrderLineItem(request);

        var orderLineItem = new OrderLineItem()
        {
        };
        return await _orderLineItemsRepository.Add(orderLineItem);
    }
}