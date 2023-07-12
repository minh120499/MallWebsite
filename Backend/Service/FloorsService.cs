using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class FloorsService
{
    private readonly IFloorsRepository _floorsRepository;

    public FloorsService(IFloorsRepository floorsRepository)
    {
        _floorsRepository = floorsRepository;
    }

    public async Task<List<Floor>> GetByFilter(FilterModel filters)
    {
        var floors = await _floorsRepository.GetByFilter(filters);
        var total = await _floorsRepository.Count();
        return floors;
    }

    public async Task<List<Floor>> Update(List<Floor> request)
    {
        foreach (var facility in request)
        {
            Validations.Floor(facility);
        }

        return await _floorsRepository.Update(request);
    }
}