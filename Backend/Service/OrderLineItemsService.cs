using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class OrderLineItemsService
    {
        private readonly IOrderLineItemsRepository _orderLineItemsRepository;

        public OrderLineItemsService(IOrderLineItemsRepository orderLineItemsRepository)
        {
            _orderLineItemsRepository = orderLineItemsRepository;
        }

        public async Task<OrderLineItem> GetById(int orderLineItemId)
        {
            return await _orderLineItemsRepository.GetById(orderLineItemId);
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
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = request.Price,
            };
            return await _orderLineItemsRepository.Add(orderLineItem);
        }

        public async Task<OrderLineItem> Update(int orderLineItemId, OrderLineItemRequest request)
        {
            Validations.OrderLineItem(request);

            return await _orderLineItemsRepository.Update(orderLineItemId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var orderLineItemIds = ids.Split(',').Select(int.Parse).ToList();

            return await _orderLineItemsRepository.Delete(orderLineItemIds);
        }
    }
}
