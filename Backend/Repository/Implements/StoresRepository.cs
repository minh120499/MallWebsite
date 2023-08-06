﻿using Backend.Exceptions;
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

        public async Task<Store> GetById(int storeId)
        {
            var store = await _context.Stores
                .Include(s => s.Banners)
                .Include(s => s.Floor)
                .Include(s => s.Banners)
                .FirstOrDefaultAsync(s => s.Id == storeId);

            if (store == null)
            {
                throw new NotFoundException("Store not found.");
            }

            return store;
        }

        public async Task<(int totalCount, List<Store>)> GetByFilter(FilterModel filters)
        {
            IQueryable<Store> query = _context.Stores
                .Include(s => s.Floor)
                .Include(s => s.Category);

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

            if (!string.IsNullOrEmpty(filters.Category))
            {
                query = query.Where(u =>
                    (u.Category != null && u.Category.Name != null && u.Category != null &&
                     u.Category.Name.Equals(filters.Category))
                    || (u.CategoryId.Equals(filters.CategoryId)));
            }

            var totalCount = await query.CountAsync();

            query = query.OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse();

            return (totalCount, await query.ToListAsync());
        }

        public async Task<List<Product>> GetProducts(int storeId, FilterModel filters)
        {
            var storeProducts = await _context.Products
                .Where(sp => sp.StoreId == storeId)
                .Include(sp => sp.Store)
                .Include(sp => sp.Variants)
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

        public async Task<Store> Update(int storeId, StoreRequest request)
        {
            try
            {
                var store = await GetById(storeId);

                store.Name = request.Name;
                if (request.FormFile != null)
                {
                    store.Image = await FileHelper.UploadImage(request.FormFile);
                }
                else
                {
                    store.Image = request.Image;
                }

                store.FloorId = request.FloorId;
                store.Phone = request.Phone;
                store.Email = request.Email;
                store.CategoryId = request.CategoryId;
                store.Facilities = request.FacilityIds;
                // store.Banners = request.Banners;
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
            return await _context.Stores
                .Where(sp => sp.Id == storeId)
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