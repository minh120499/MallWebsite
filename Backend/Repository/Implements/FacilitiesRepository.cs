using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class FacilitiesRepository : IFacilitiesRepository
    {
        private readonly ApplicationDbContext _context;

        public FacilitiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Facility> GetById(int facilityId)
        {
            var facility = await _context.Facilities.FindAsync(facilityId);

            if (facility == null)
            {
                throw new NotFoundException("Facility not found.");
            }

            return facility;
        }

        public async Task<List<Facility>> GetByFilter(FilterModel filters)
        {
            var facilities = await _context.Facilities
                .Where(u => u.Name != null &&
                            u.Name!.Contains(filters.Query ?? "") &&
                            u.Status!.Equals(StatusConstraint.ACTIVE))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return facilities;
        }

        public async Task<Facility> Add(Facility facility)
        {
            try
            {
                facility.CreateOn = DateTime.Now;
                facility.ModifiedOn = DateTime.Now;
                await _context.Facilities.AddAsync(facility);
                await _context.SaveChangesAsync();

                return facility;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Facility> Update(int facilityId, FacilityRequest request)
        {
            try
            {
                var facility = await GetById(facilityId);

                facility.Name = request.Name;
                facility.Status = request.Status;
                facility.Description = request.Description;
                facility.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return facility;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Facility>> Update(List<Facility> facilities)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var facility in facilities)
                {
                    if (facility.Id != 0)
                    {
                        var findFacility = await _context.Facilities.FindAsync(facility.Id);
                        if (findFacility == null) continue;

                        findFacility.Name = facility.Name;
                        findFacility.Status = facility.Status;
                        findFacility.Description = facility.Description;

                        findFacility.ModifiedOn = DateTime.Now;
                        _context.Facilities.Update(findFacility);
                    }
                    else
                    {
                        facility.Status = StatusConstraint.ACTIVE;
                        facility.CreateOn = DateTime.Now;
                        facility.ModifiedOn = DateTime.Now;
                        await _context.Facilities.AddAsync(facility);
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
            return await _context.Facilities.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var facilities = await _context.Facilities.Where(f => ids.Contains(f.Id)).ToListAsync();
                _context.Facilities.RemoveRange(facilities);
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