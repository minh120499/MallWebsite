using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IFloorsRepository
{
    public Task<List<Floor>> GetByFilter(FilterModel filters);
    public Task<List<Floor>> Update(List<Floor> banner);
    public Task<int> Count();
}