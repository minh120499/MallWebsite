using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class FacilitiesService
    {
        private readonly IFacilitiesRepository _facilitiesRepository;

        public FacilitiesService(IFacilitiesRepository facilitiesRepository)
        {
            _facilitiesRepository = facilitiesRepository;
        }

        public async Task<Facility> GetById(int facilityId)
        {
            return await _facilitiesRepository.GetById(facilityId);
        }

        public async Task<TableListResponse<Facility>> GetByFilter(FilterModel filters)
        {
            var facilities = await _facilitiesRepository.GetByFilter(filters);
            var total = await _facilitiesRepository.Count();
            return new TableListResponse<Facility>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = facilities
            };
        }

        public async Task<Facility> Create(FacilityRequest request)
        {
            Validations.Facility(request);

            var facility = new Facility()
            {
                Name = request.Name,
            };
            return await _facilitiesRepository.Add(facility);
        }

        public async Task<Facility> Update(int facilityId, FacilityRequest request)
        {
            Validations.Facility(request);

            return await _facilitiesRepository.Update(facilityId, request);
        }

        public async Task<List<Facility>> Update(List<Facility> facilities)
        {
            foreach (var facility in facilities)
            {
                Validations.Facility(facility);
            }

            return await _facilitiesRepository.Update(facilities);
        }

        public async Task<bool> Delete(string ids)
        {
            var facilityIds = ids.Split(',').Select(int.Parse).ToList();

            return await _facilitiesRepository.Delete(facilityIds);
        }
    }
}