using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class StoresService
    {
        private readonly IStoresRepository _storesRepository;
        private readonly IBannersRepository _bannersRepository;

        public StoresService(IStoresRepository storesRepository, IBannersRepository bannersRepository)
        {
            _storesRepository = storesRepository;
            _bannersRepository = bannersRepository;
        }

        public async Task<StoreResponse> GetById(int storeId)
        {
            return await _storesRepository.GetById(storeId);
        }

        public async Task<TableListResponse<Store>> GetByFilter(FilterModel filters)
        {
            var stores = await _storesRepository.GetByFilter(filters);
            var total = await _storesRepository.Count();
            return new TableListResponse<Store>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = stores
            };
        }

        public async Task<StoreResponse> Create(StoreRequest request)
        {
            Validations.Store(request);

            var store = new Store()
            {
                Name = request.Name,
                Image = request.Image,
                FloorId = request.FloorId,
                CategoryId = request.CategoryId,
                Facilities = request.FacilityIds,
                Status = request.Status,
                Description = request.Description,
            };
            
            var storeEntity = await _storesRepository.Add(store);
            // await _bannersRepository.Update(new BannerRequest()
            // {
            //     Id = request.Banners
            // });
            return await _storesRepository.GetById(storeEntity.Id);
        }

        public async Task<StoreResponse> Update(int storeId, StoreRequest request)
        {
            Validations.Store(request);

            return await _storesRepository.Update(storeId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var storeIds = ids.Split(',').Select(int.Parse).ToList();

            return await _storesRepository.Delete(storeIds);
        }
    }
}