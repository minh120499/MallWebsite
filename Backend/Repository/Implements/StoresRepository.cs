using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class StoresRepository : IStoresRepository
{
    private readonly ApplicationDbContext _context;

    public StoresRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Store>> GetByFilter(FilterModel filters)
    {
        var banners = await _context.Stores
            // .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return banners;
    }

    public async Task<Store> Add(Store store)
    {
        try
        {
            store.CreateOn = DateTime.Now;
            store.ModifiedOn = DateTime.Now;
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
            
            return store;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}