using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class OrdersService
{
    private readonly IOrdersRepository _ordersRepository;

    public OrdersService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
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
            Status = StatusConstraint.ACTIVE,
        };
        return await _ordersRepository.Add(order);
    }
}