using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IOrdersRepository
{
    public Task<List<Order>> GetByFilter(FilterModel filters);
    public Task<Order> Add(Order banner);
    public Task<int> Count();
}