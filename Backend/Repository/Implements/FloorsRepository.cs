using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class FloorsRepository : IFloorsRepository
    {
        private readonly ApplicationDbContext _context;

        public FloorsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Floor> GetById(int floorId)
        {
            var floor = await _context.Floors.FindAsync(floorId);

            if (floor == null)
            {
                throw new NotFoundException("Floor not found.");
            }

            return floor;
        }

        public async Task<List<Floor>> GetByFilter(FilterModel filters)
        {
            var floors = await _context.Floors
                .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return floors;
        }

        public async Task<Floor> Add(Floor floor)
        {
            try
            {
                floor.CreateOn = DateTime.Now;
                floor.ModifiedOn = DateTime.Now;
                await _context.Floors.AddAsync(floor);
                await _context.SaveChangesAsync();

                return floor;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Floor> Update(int floorId, FloorRequest request)
        {
            try
            {
                var floor = await GetById(floorId);

                floor.Name = request.Name;
                floor.Area = request.Area;
                floor.Description = request.Description;
                floor.Status = request.Status;
                floor.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return floor;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Floor>> Update(List<Floor> floors)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var floor in floors)
                {
                    if (floor.Id != 0)
                    {
                        var findFloor = await _context.Facilities.FindAsync(floor.Id);
                        if (findFloor == null) continue;

                        findFloor.Name = floor.Name;
                        findFloor.Status = floor.Status;
                        findFloor.ModifiedOn = DateTime.Now;
                        _context.Facilities.Update(findFloor);
                    }
                    else
                    {
                        floor.Status = StatusConstraint.ACTIVE;
                        floor.CreateOn = DateTime.Now;
                        floor.ModifiedOn = DateTime.Now;
                        await _context.Floors.AddAsync(floor);
                    }
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetByFilter(new FilterModel());
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.Floors.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var floors = await _context.Floors.Where(f => ids.Contains(f.Id)).ToListAsync();
                _context.Floors.RemoveRange(floors);
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