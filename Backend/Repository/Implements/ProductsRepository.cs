using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Repository.Implements
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoriesRepository _categoriesRepository;

        public ProductsRepository(ApplicationDbContext context, ICategoriesRepository categoriesRepository)
        {
            _context = context;
            _categoriesRepository = categoriesRepository;
        }

        public async Task<Product> GetById(int productId)
        {
            var products = await _context.Products
                .Include(p => p.ProductCategory)!
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (products == null)
            {
                throw new NotFoundException("Product not found.");
            }

            return products;
        }

        public async Task<List<Product>> GetByFilter(FilterModel filters)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Variants)
                .Include(p => p.ProductCategory)!
                .ThenInclude(c => c.Category);

            if (filters.Ids.Any())
            {
                query = query.Where(u => filters.Ids.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(filters.Status))
            {
                query = query.Where(u => u.Status == filters.Status);
            }

            if (!string.IsNullOrEmpty(filters.Query))
            {
                query = query.Where(u => u.Name != null && u.Name.Contains(filters.Query));
            }

            query = query.OrderByDescending(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse();

            var products = await query.ToListAsync();

            foreach (var product in products)
            {
                foreach (var productCategory in product.ProductCategory!)
                {
                    productCategory.Product = null;
                }

                product.Store = null;
            }

            return products;
        }

        public async Task<Product> Add(ProductRequest request)
        {
            try
            {
                var image = await FileHelper.UploadImage(request.FormFile);
                var product = new Product()
                {
                    Code = request.Code,
                    Image = image,
                    Name = request.Name,
                    StoreId = request.StoreId,
                    Description = request.Description,
                    Brand = request.Brand,
                };

                if (request.Categories != null)
                {
                    var categoriesIds = JsonConvert.DeserializeObject<List<int>>(request.CategoriesIds)!;
                    var categories = _categoriesRepository.GetByFilter(new FilterModel()
                    {
                        Ids = categoriesIds
                    }).Result.Item2;

                    if (categories == null || categories.Count == 0 || categories.Count != categoriesIds.Count)
                    {
                        throw new NotFoundException("categories not found");
                    }

                    foreach (var category in categories)
                    {
                        product.ProductCategory = new List<ProductCategory>
                            { new() { Category = category, CategoryId = category.Id } };
                    }
                }

                if (request.Variants == null)
                {
                    product.Variants = new List<Variant>
                    {
                        new()
                        {
                            Name = request.Name,
                            Code = request.Code,
                            Description = request.Description,
                            Image = image,
                            Price = request.Price,
                            InStock = request.InStock,
                            CreateOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        }
                    };
                }

                product.CreateOn = DateTime.Now;
                product.ModifiedOn = DateTime.Now;
                product.Status = StatusConstraint.ACTIVE;

                await _context.Variants.AddRangeAsync(product.Variants!);
                var response = await _context.Products.AddAsync(product);
                if (response.Entity.ProductCategory != null)
                {
                    await _context.ProductCategories.AddRangeAsync(response.Entity.ProductCategory);
                }

                await _context.SaveChangesAsync();
                return response.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Product> Update(int productId, ProductRequest request)
        {
            try
            {
                var product = await GetById(productId);

                product.Code = request.Code;
                product.Image = await FileHelper.UploadImage(request.FormFile);
                product.Name = request.Name;
                product.Description = request.Description;
                product.Brand = request.Brand;
                product.StoreId = request.StoreId;
                product.Variants = request.Variants;
                product.Status = request.Status;
                product.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var products = await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
                _context.Products.RemoveRange(products);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}