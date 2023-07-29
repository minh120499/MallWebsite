using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
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
        IQueryable<Banner> query = _context.Banners
            .Include(u => u.Store);

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

        query = query.OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .Reverse();
        return await query.ToListAsync();
    }

    public async Task<Banner> Add(Banner banner)
    {
        try
        {
            banner.CreateOn = DateTime.Now;
            banner.ModifiedOn = DateTime.Now;
            banner.Status = StatusConstraint.ACTIVE;
            if (banner is { StartOn: { }, EndOn: { } })
            {
                banner.Expire = (banner.EndOn - banner.StartOn).Value.Days;
            }

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
            banner.Image = await FileHelper.UploadImage(request.FormFile);
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