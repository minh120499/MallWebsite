using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategory)!
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            return product;
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

            if (!string.IsNullOrEmpty(filters.Type))
            {
                query = query.Where(u => u.Name != null && u.Name.Equals(filters.Type));
            }

            query = query.OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse();
            
            return await query.ToListAsync();
        }

        public async Task<Product> Add(Product product)
        {
            try
            {
                product.CreateOn = DateTime.Now;
                product.ModifiedOn = DateTime.Now;
                product.Status = StatusConstraint.ACTIVE;
                var response = await _context.Products.AddAsync(product);
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
                // product.Variants = request.Variants;
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