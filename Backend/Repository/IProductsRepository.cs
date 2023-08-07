using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IProductsRepository
{
    Task<Product> GetById(int productId);
    Task<List<Product>> GetByFilter(FilterModel filters);
    Task<Product> Add(ProductRequest request);
    Task<Product> Update(int productId, ProductRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}