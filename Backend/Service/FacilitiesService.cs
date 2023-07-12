using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class FacilitiesService
{
    private readonly IFacilitiesRepository _facilitiesRepository;

    public FacilitiesService(IFacilitiesRepository facilitiesRepository)
    {
        _facilitiesRepository = facilitiesRepository;
    }

    public async Task<List<Facility>> GetByFilter(FilterModel filters)
    {
        var facilities = await _facilitiesRepository.GetByFilter(filters);
        var total = await _facilitiesRepository.Count();
        return facilities;
    }

    public async Task<List<Facility>> Update(List<Facility> request)
    {
        foreach (var facility in request)
        {
            Validations.Facility(facility);
        }

        return await _facilitiesRepository.Update(request);
    }
}