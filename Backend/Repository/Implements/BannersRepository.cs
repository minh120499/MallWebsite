using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class BannersRepository : IBannersRepository
{
    private readonly ApplicationDbContext _context;

    public BannersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Banner> GetById(int bannerId)
    {
        var banner = await _context.Banners.FindAsync(bannerId);

        if (banner == null)
        {
            throw new NotFoundException("Banner not found.");
        }

        return banner;
    }

    public async Task<List<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _context.Banners
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .Reverse()
            .ToListAsync();
        return banners;
    }

    public async Task<Banner> Add(Banner banner)
    {
        try
        {
            banner.CreateOn = DateTime.Now;
            banner.ModifiedOn = DateTime.Now;
            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();

            return banner;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Banner> Update(int bannerId, BannerRequest request)
    {
        try
        {
            var banner = await GetById(bannerId);

            banner.Name = request.Name;
            banner.Image = request.Image;
            banner.Expire = request.Expire;
            banner.Status = request.Status;
            banner.StoreId = request.StoreId;
            banner.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return banner;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
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