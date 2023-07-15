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

        public async Task<TableListResponse<Product>> GetByFilter(FilterModel filters)
        {
            var products = await _productsRepository.GetByFilter(filters);
            var total = await _productsRepository.Count();
            return new TableListResponse<Product>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = products
            };
        }

        public async Task<Product> Create(ProductRequest request)
        {
            Validations.Product(request);

            var product = new Product()
            {
                Name = request.Name,
                // Price = request.Price,
                Description = request.Description,
            };
            return await _productsRepository.Add(product);
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