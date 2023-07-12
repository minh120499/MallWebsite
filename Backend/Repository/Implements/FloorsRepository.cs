using System.Transactions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class FloorsRepository : IFloorsRepository
{
    private readonly ApplicationDbContext _context;

    public FloorsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Floor>> GetByFilter(FilterModel filters)
    {
        var floors = await _context.Floors
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return floors;
    }

    public async Task<List<Floor>> Update(List<Floor> floors)
    {
        using (var scope = new TransactionScope())
        {
            try
            {
                foreach (var floor in floors)
                {
                    if (floor.Id == 0)
                    {
                        floor.CreateOn = DateTime.Now;
                        floor.ModifiedOn = DateTime.Now;
                        floor.Status = StatusConstraint.ACTIVE;
                        await _context.Floors.AddAsync(floor);
                    }
                    else
                    {
                        floor.ModifiedOn = DateTime.Now;
                        _context.Floors.Update(floor);
                    }
                }

                await _context.SaveChangesAsync();

                scope.Complete();

                return floors;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public async Task<int> Count()
    {
        return await _context.Floors.CountAsync();
    }
}