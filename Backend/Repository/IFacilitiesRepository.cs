using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IFacilitiesRepository
{
    public Task<List<Facility>> GetByFilter(FilterModel filters);
    public Task<List<Facility>> Update(List<Facility> facilities);
    public Task<int> Count();
}