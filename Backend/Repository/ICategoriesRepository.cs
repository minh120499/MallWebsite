using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface ICategoriesRepository
{
    public Task<List<Category>> GetByFilter(FilterModel filters);
    public Task<Category> Add(Category banner);
    public Task<int> Count();
}