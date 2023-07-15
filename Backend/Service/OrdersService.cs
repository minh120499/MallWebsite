using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class OrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Order> GetById(int orderId)
        {
            return await _ordersRepository.GetById(orderId);
        }

        public async Task<TableListResponse<Order>> GetByFilter(FilterModel filters)
        {
            var orders = await _ordersRepository.GetByFilter(filters);
            var total = await _ordersRepository.Count();
            return new TableListResponse<Order>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = orders
            };
        }

        public async Task<Order> Create(OrderRequest request)
        {
            Validations.Order(request);

            var order = new Order()
            {
                // CustomerName = request.CustomerName,
                // TotalAmount = request.TotalAmount,
            };
            return await _ordersRepository.Add(order);
        }

        public async Task<Order> Update(int orderId, OrderRequest request)
        {
            Validations.Order(request);

            return await _ordersRepository.Update(orderId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var orderIds = ids.Split(',').Select(int.Parse).ToList();

            return await _ordersRepository.Delete(orderIds);
        }
    }
}