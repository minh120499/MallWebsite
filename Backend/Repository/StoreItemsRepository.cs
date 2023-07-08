using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class StoreItemsRepository : IStoreItemsRepository
{
    private readonly ApplicationDbContext _context;

    public StoreItemsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StoreItem>> GetByFilter(FilterModel filters)
    {
        var storeItems = await _context.StoreItems
            .Include(si => si.Store)
            .Where(si => si.Name != null && si.Name.Contains(filters.Query ?? ""))
            .OrderBy(si => si.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return storeItems;
    }

    public async Task<StoreItem> Add(StoreItem storeItem)
    {
        try
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id.Equals(storeItem.StoreId));
            if (store == null)
            {
                throw new NotFoundException("store not found");
            }

            storeItem.CreateOn = DateTime.Now;
            storeItem.ModifiedOn = DateTime.Now;
            storeItem.Store = store;
            await _context.StoreItems.AddAsync(storeItem);
            await _context.SaveChangesAsync();

            return storeItem;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}