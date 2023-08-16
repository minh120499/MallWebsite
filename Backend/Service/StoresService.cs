using Backend.Exceptions;
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
        private readonly IProductsRepository _productsRepository;

        public StoresService(IStoresRepository storesRepository, IBannersRepository bannersRepository, IProductsRepository productsRepository)
        {
            _storesRepository = storesRepository;
            _bannersRepository = bannersRepository;
            _productsRepository = productsRepository;
        }

        public async Task<Store> GetById(int storeId)
        {
            return await _storesRepository.GetById(storeId);
        }

        public async Task<TableListResponse<Store>> GetByFilter(FilterModel filters)
        {
            var stores = await _storesRepository.GetByFilter(filters);
            return new TableListResponse<Store>()
            {
                Total = stores.totalCount,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = stores.Item2
            };
        }

        public async Task<Store> Create(StoreRequest request)
        {
            Validations.Store(request);

            var image = await FileHelper.UploadImage(request.FormFile);
            var store = new Store()
            {
                Name = request.Name,
                Image = image,
                Phone = request.Phone,
                Email = request.Email,
                FloorId = request.FloorId,
                CategoryId = request.CategoryId,
                Facilities = request.FacilityIds,
                Status = request.Status,
                Description = request.Description,
            };


            Store storeEntity;
            if (request.BannersIds is null or "")
            {
                storeEntity = await _storesRepository.Add(store);
                return await _storesRepository.GetById(storeEntity.Id);
            }

            var ids = request.BannersIds.Split(",").Select(int.Parse).ToList();

            var banners = await _bannersRepository.GetByFilter(new FilterModel()
            {
                Ids = ids
            });

            if (banners.totalCount == 0)
            {
                throw new NotFoundException("Banner not not exists");
            }

            storeEntity = await _storesRepository.Add(store);
            await _bannersRepository.UpdateByStore(banners.Item2, storeEntity.Id);

            return await _storesRepository.GetById(storeEntity.Id);
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

        public async Task<TableListResponse<Product>> GetProducts(int id, FilterModel filters)
        {
            var products = await _storesRepository.GetProducts(id, filters);
            
            return new TableListResponse<Product>()
            {
                Data = products.Item2,
                Limit = filters.Limit,
                Page = filters.Page,
                Total = products.totalCount
            };
        }
    }
}