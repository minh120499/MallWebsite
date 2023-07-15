using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IOrdersRepository
{
    Task<Order> GetById(int orderId);
    Task<List<Order>> GetByFilter(FilterModel filters);
    Task<Order> Add(Order order);
    Task<Order> Update(int orderId, OrderRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}