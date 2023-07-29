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

            if (ids.Count != banners.Count)
            {
                throw new NotFoundException("Banner not not exists");
            }

            storeEntity = await _storesRepository.Add(store);
            await _bannersRepository.UpdateByStore(banners, storeEntity.Id);

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

        public async Task<TableListResponse<StoreProductResponse>> GetProducts(int id, FilterModel filters)
        {
            var storeProduct = await _storesRepository.GetProducts(id, filters);
            var total = await _storesRepository.CountProducts(storeProduct.First().StoreId);
            var storeProductResponse = storeProduct.Select((sp) => new StoreProductResponse()
            {
                Id = sp.Id,
                Code = sp.Product!.Code,
                Image = sp.Product!.Image,
                Name = sp.Product!.Name,
                Description = sp.Product!.Description,
                Brand = sp.Product!.Brand,
                ProductCategory = sp.Product!.ProductCategory,
                Variants = sp.Product!.Variants,
                Status = sp.Product!.Status,
                CreateOn = sp.Product!.CreateOn,
                ModifiedOn = sp.Product!.ModifiedOn,
                Store = sp.Store,
            }).ToList();

            return new TableListResponse<StoreProductResponse>()
            {
                Data = storeProductResponse,
                Limit = filters.Limit,
                Page = filters.Page,
                Total = total
            };
        }
    }
}