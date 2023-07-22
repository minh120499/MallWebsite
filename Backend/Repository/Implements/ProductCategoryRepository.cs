using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ProductCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory> GetById(int bannerId)
    {
        var banner = await _context.ProductCategories.FindAsync(bannerId);

        if (banner == null)
        {
            throw new NotFoundException("Banner not found.");
        }

        return banner;
    }

    public async Task<List<ProductCategory>> GetByFilter(FilterModel filters)
    {
        IQueryable<ProductCategory> query = _context.ProductCategories;


        query = query
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .Reverse();
        return await query.ToListAsync();
    }

    public async Task<List<ProductCategory>> Add(List<ProductCategory> productCategories)
    {
        await _context.ProductCategories.AddRangeAsync(productCategories);
        await _context.SaveChangesAsync(); // Save changes to the database

        return productCategories;
    }


    public Task<ProductCategory> Update(int bannerId, ProductCategory request)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductCategory>> UpdateByStore(List<ProductCategory> productCategories, int storeId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductCategory> Update(ProductCategory request)
    {
        try
        {
            await _context.SaveChangesAsync();

            return request;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<Banner>> UpdateByStore(List<Banner> banners, int storeId)
    {
        foreach (var banner in banners)
        {
            banner.StoreId = storeId;
        }

        _context.Banners.UpdateRange(banners);
        await _context.SaveChangesAsync();

        return banners;
    }

    public async Task<int> Count()
    {
        return await _context.Banners.CountAsync();
    }

    public async Task<bool> Delete(List<int> ids)
    {
        try
        {
            var bannerIds = await _context.Banners.Where(b => ids.Contains(b.Id)).ToListAsync();
            _context.Banners.RemoveRange(bannerIds);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}