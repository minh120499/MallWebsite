using System.Transactions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class FacilitiesRepository : IFacilitiesRepository
{
    private readonly ApplicationDbContext _context;

    public FacilitiesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Facility>> GetByFilter(FilterModel filters)
    {
        var facilities = await _context.Facilities
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return facilities;
    }

    public async Task<List<Facility>> Update(List<Facility> facilities)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var facility in facilities)
            {
                if (facility.Id == 0)
                {
                    facility.CreateOn = DateTime.Now;
                    facility.ModifiedOn = DateTime.Now;
                    // facility.Status = StatusConstraint.ACTIVE;
                    await _context.Facilities.AddAsync(facility);
                }
                else
                {
                    facility.ModifiedOn = DateTime.Now;
                    _context.Facilities.Update(facility);
                }
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return facilities;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public async Task<int> Count()
    {
        return await _context.Facilities.CountAsync();
    }
}