using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class StoreProductsRepository : IStoreProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public StoreProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoreProduct> GetById(int storeProductId)
        {
            var storeProduct = await _context.StoreProducts.FindAsync(storeProductId);

            if (storeProduct == null)
            {
                throw new NotFoundException("Store Product not found.");
            }

            return storeProduct;
        }

        public async Task<List<StoreProduct>> GetByFilter(FilterModel filters)
        {
            var storeProducts = await _context.StoreProducts
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return storeProducts;
        }

        public async Task<StoreProduct> Add(StoreProduct storeProduct)
        {
            try
            {
                storeProduct.CreateOn = DateTime.Now;
                storeProduct.ModifiedOn = DateTime.Now;
                storeProduct.Status = StatusConstraint.ACTIVE;
                var response = await _context.StoreProducts.AddAsync(storeProduct);
                await _context.SaveChangesAsync();

                return response.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<StoreProduct> Update(int storeProductId, StoreProductRequest request)
        {
            try
            {
                var storeProduct = await GetById(storeProductId);

                storeProduct.StoreId = request.StoreId;
                storeProduct.Store = request.Store;
                storeProduct.ProductId = request.ProductId;
                storeProduct.Product = request.Product;
                storeProduct.Variants = request.Variants!;
                storeProduct.Status = request.Status;
                storeProduct.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return storeProduct;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.StoreProducts.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var storeProducts = await _context.StoreProducts.Where(s => ids.Contains(s.Id)).ToListAsync();
                _context.StoreProducts.RemoveRange(storeProducts);
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