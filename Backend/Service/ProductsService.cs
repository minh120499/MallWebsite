using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;
using Newtonsoft.Json;

namespace Backend.Service
{
    public class ProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStoreProductsRepository _storeProductsRepository;
        private readonly IVariantsRepository _variantsRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductsService(IProductsRepository productsRepository,
            IProductCategoryRepository productCategoryRepository, IStoreProductsRepository storeProductsRepository,
            IVariantsRepository variantsRepository)
        {
            _productsRepository = productsRepository;
            _productCategoryRepository = productCategoryRepository;
            _storeProductsRepository = storeProductsRepository;
            _variantsRepository = variantsRepository;
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
            var image = await FileHelper.UploadImage(request.FormFile);
            var product = new Product()
            {
                Code = request.Code,
                Image = image,
                Name = request.Name,
                Description = request.Description,
                Brand = request.Brand,
            };

            var productResponse = await _productsRepository.Add(product);
            if (request.Categories != null)
            {
                var categoriesIds = JsonConvert.DeserializeObject<List<string>>(request.CategoriesIds)!;
                var productCategory = categoriesIds
                    .Select(c => new ProductCategory
                    {
                        ProductId = productResponse.Id,
                        CategoryId = Convert.ToInt32(c)
                    }).ToList();
                await _productCategoryRepository.Add(productCategory);
            }

            if (request.Variant == null)
            {
                await _variantsRepository.Add(new Variant()
                {
                    Name = request.Name,
                    ProductId = productResponse.Id,
                    Code = request.Code,
                    Description = request.Description,
                    Image = image,
                    Price = request.Price,
                    InStock = request.InStock,
                });
            }

            await _storeProductsRepository.Add(new StoreProduct()
            {
                StoreId = request.StoreId,
                ProductId = productResponse.Id,
            });


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