using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class VariantsRepository : IVariantsRepository
    {
        private readonly ApplicationDbContext _context;

        public VariantsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Variant> GetById(int variantId)
        {
            var variant = await _context.Variants.FindAsync(variantId);

            if (variant == null)
            {
                throw new NotFoundException("Variant not found.");
            }

            return variant;
        }

        public async Task<List<Variant>> GetByFilter(FilterModel filters)
        {
            var variants = await _context.Variants
                .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return variants;
        }

        public async Task<Variant> Add(Variant variant)
        {
            try
            {
                variant.CreateOn = DateTime.Now;
                variant.ModifiedOn = DateTime.Now;
                var response = await _context.Variants.AddAsync(variant);
                await _context.SaveChangesAsync();

                return response.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Variant> Update(int variantId, VariantRequest request)
        {
            try
            {
                var variant = await GetById(variantId);

                variant.ProductId = request.ProductId;
                variant.Product = request.Product;
                variant.Image = await FileHelper.UploadImage(request.FormFile);
                variant.Code = request.Code;
                variant.Name = request.Name;
                variant.Options = request.Options;
                variant.Description = request.Description;
                variant.ModifiedOn = DateTime.Now;
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

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var variants = await _context.Variants.Where(v => ids.Contains(v.Id)).ToListAsync();
                _context.Variants.RemoveRange(variants);
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