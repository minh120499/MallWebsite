using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IProductCategoryRepository
{
    Task<ProductCategory> GetById(int bannerId);
    public Task<List<ProductCategory>> GetByFilter(FilterModel filters);
    public Task<List<ProductCategory>> Add(List<ProductCategory> productCategories);
    public Task<ProductCategory> Update(int bannerId, ProductCategory request);
    public Task<List<ProductCategory>> UpdateByStore(List<ProductCategory> productCategories, int storeId);
    public Task<int> Count();
    public Task<bool> Delete(List<int> ids);
}