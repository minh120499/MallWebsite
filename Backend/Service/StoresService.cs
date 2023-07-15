using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;
using System.Threading.Tasks;

namespace Backend.Service
{
    public class StoresService
    {
        private readonly IStoresRepository _storesRepository;

        public StoresService(IStoresRepository storesRepository)
        {
            _storesRepository = storesRepository;
        }

        public async Task<Store> GetById(int storeId)
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

        public async Task<Store> Create(StoreRequest request)
        {
            Validations.Store(request);

            var store = new Store()
            {
                Name = request.Name,
                // Location = request.Location,
            };
            return await _storesRepository.Add(store);
        }

        public async Task<Store> Update(int storeId, StoreRequest request)
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