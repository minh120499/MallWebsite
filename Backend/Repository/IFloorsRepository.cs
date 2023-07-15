using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IFloorsRepository
{
    Task<Floor> GetById(int floorId);
    Task<List<Floor>> GetByFilter(FilterModel filters);
    Task<Floor> Add(Floor floor);
    Task<Floor> Update(int floorId, FloorRequest request);
    Task<List<Floor>> Update(List<Floor> floors);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}