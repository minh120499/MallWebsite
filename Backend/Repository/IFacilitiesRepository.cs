using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IFacilitiesRepository
{
    Task<Facility> GetById(int facilityId);
    Task<List<Facility>> GetByFilter(FilterModel filters);
    Task<Facility> Add(Facility facility);
    Task<Facility> Update(int facilityId, FacilityRequest request);
    Task<List<Facility>> Update(List<Facility> facilities);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}