using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IProductsRepository
{
    public Task<List<Product>> GetByFilter(FilterModel filters);
    public Task<Product> Add(Product banner);
    public Task<int> Count();
}