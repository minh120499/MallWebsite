using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class FloorsService
    {
        private readonly IFloorsRepository _floorsRepository;

        public FloorsService(IFloorsRepository floorsRepository)
        {
            _floorsRepository = floorsRepository;
        }

        public async Task<Floor> GetById(int floorId)
        {
            return await _floorsRepository.GetById(floorId);
        }

        public async Task<TableListResponse<Floor>> GetByFilter(FilterModel filters)
        {
            var floors = await _floorsRepository.GetByFilter(filters);
            var total = await _floorsRepository.Count();
            return new TableListResponse<Floor>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = floors
            };
        }

        public async Task<Floor> Create(FloorRequest request)
        {
            Validations.Floor(request);

            var floor = new Floor()
            {
                Name = request.Name,
                Description = request.Description,
            };
            return await _floorsRepository.Add(floor);
        }

        public async Task<Floor> Update(int floorId, FloorRequest request)
        {
            Validations.Floor(request);

            return await _floorsRepository.Update(floorId, request);
        }
        
        public async Task<List<Floor>> Update(List<Floor> floors)
        {
            foreach (var floor in floors)
            {
                Validations.Floor(floor);
            }

            return await _floorsRepository.Update(floors);
        }

        public async Task<bool> Delete(string ids)
        {
            var floorIds = ids.Split(',').Select(int.Parse).ToList();

            return await _floorsRepository.Delete(floorIds);
        }
    }
}