using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class BannersRepository : IBannersRepository
{
    private readonly ApplicationDbContext _context;

    public BannersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _context.Banners
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
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

    public async Task<int> Count()
    {
        return await _context.Banners.CountAsync();
    }
}