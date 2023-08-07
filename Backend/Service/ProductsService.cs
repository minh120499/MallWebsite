using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class ProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<Product> GetById(int productId)
        {
            return await _productsRepository.GetById(productId);
        }

        public async Task<TableListResponse<ProductResponse>> GetByFilter(FilterModel filters)
        {
            var products = await _productsRepository.GetByFilter(filters);
            var productsResponse = products.Select((p) => new ProductResponse()
            {
                Id = p.Id,
                Code = p.Code,
                Image = p.Image,
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Categories = p.ProductCategory!.Select(pc => pc.Category!.Name ?? "").ToList(),
                Variants = p.Variants!.Select(v =>
                {
                    v.Product = null;
                    return v;
                }).ToList(),
                Status = p.Status,
                CreateOn = p.CreateOn,
                ModifiedOn = p.ModifiedOn,
            }).ToList();
            var total = await _productsRepository.Count();
            return new TableListResponse<ProductResponse>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = productsResponse
            };
        }

        public async Task<Product> Create(ProductRequest request)
        {
            Validations.Product(request);
            var productResponse = await _productsRepository.Add(request);

            return await _productsRepository.GetById(productResponse.Id);
        }

        public async Task<Product> Update(int productId, ProductRequest request)
        {
            Validations.Product(request);

            return await _productsRepository.Update(productId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var productIds = ids.Split(',').Select(int.Parse).ToList();

            return await _productsRepository.Delete(productIds);
        }
    }
}