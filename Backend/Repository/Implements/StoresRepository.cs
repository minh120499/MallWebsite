using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class StoresRepository : IStoresRepository
    {
        private readonly ApplicationDbContext _context;

        public StoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoreResponse> GetById(int storeId)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                throw new NotFoundException("Store not found.");
            }

            return new StoreResponse();
        }

        public async Task<List<Store>> GetByFilter(FilterModel filters)
        {
            var stores = await _context.Stores
                .Include(s => s.Floor)
                .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return stores;
        }

        public async Task<List<StoreProduct>> GetProducts(int storeId, FilterModel filters)
        {
            var storeProducts = await _context.StoreProducts
                .Where(sp => sp.StoreId == storeId)
                .Include(sp => sp.Product)
                .ThenInclude(p => p!.Variants)
                .Include(sp => sp.Store)
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return storeProducts;
        }

        public async Task<Store> Add(Store store)
        {
            try
            {
                store.CreateOn = DateTime.Now;
                store.ModifiedOn = DateTime.Now;
                store.Status = StatusConstraint.ACTIVE;
                var entity = await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();

                return entity.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<StoreResponse> Update(int storeId, StoreRequest request)
        {
            try
            {
                var store = await GetById(storeId);

                store.Name = request.Name;
                store.Image = request.Image;
                store.Floor = request.Floor;
                store.Phone = request.Phone;
                store.Email = request.Email;
                store.Category = request.Category;
                // store.Facilities = request.FacilityIds;
                store.Banners = request.Banners;
                store.Description = request.Description;
                store.Status = request.Status;
                store.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return await GetById(storeId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.Stores.CountAsync();
        }

        public async Task<int> CountProducts(int storeId)
        {
            return await _context.StoreProducts
                .Where(sp => sp.StoreId == storeId)
                .CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var stores = await _context.Stores.Where(s => ids.Contains(s.Id)).ToListAsync();
                _context.Stores.RemoveRange(stores);
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