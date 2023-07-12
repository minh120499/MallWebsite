using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IVariantsRepository
{
    public Task<List<Variant>> GetByFilter(FilterModel filters);
    public Task<Variant> Add(Variant banner);
    public Task<int> Count();
}