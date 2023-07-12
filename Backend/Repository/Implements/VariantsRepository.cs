using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class VariantsRepository : IVariantsRepository
{
    private readonly ApplicationDbContext _context;

    public VariantsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Variant>> GetByFilter(FilterModel filters)
    {
        var variants = await _context.Variants
            // .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return variants;
    }

    public async Task<Variant> Add(Variant variant)
    {
        try
        {
            variant.CreateOn = DateTime.Now;
            await _context.Variants.AddAsync(variant);
            await _context.SaveChangesAsync();

            return variant;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Count()
    {
        return await _context.Variants.CountAsync();
    }
}