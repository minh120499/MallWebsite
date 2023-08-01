using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface ICategoriesRepository
{
    Task<Category> GetById(int bannerId);
    public Task<(int totalCount, List<Category>)> GetByFilter(FilterModel filters);
    public Task<Category> Add(Category banner);
    public Task<Category> Update(int categoryId, CategoryRequest request);
    public Task<int> Count();
    public Task<bool> Delete(List<int> ids);
}