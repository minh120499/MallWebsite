using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category> GetById(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);

        if (category == null)
        {
            throw new NotFoundException("Category not found.");
        }

        return category;
    }

    public async Task<(int totalCount, List<Category>)> GetByFilter(FilterModel filters)
    {
        IQueryable<Category> query = _context.Categories
            .Where(b => b.Status != StatusConstraint.DELETED);

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

        if (!string.IsNullOrEmpty(filters.Type))
        {
            query = query.Where(u => u.Type != null && u.Type.Equals(filters.Type));
        }

        var totalCount = await query.CountAsync();

        query = query.OrderByDescending(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit);

        return (totalCount, await query.ToListAsync());
    }

    public async Task<Category> Add(Category category)
    {
        try
        {
            category.CreateOn = DateTime.Now;
            category.ModifiedOn = DateTime.Now;
            category.ProductCategory = new List<ProductCategory>();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Category> Update(int categoryId, CategoryRequest request)
    {
        try
        {
            var category = await GetById(categoryId);

            category.Name = request.Name;
            if (request.FormFile != null)
            {
                category.Image = await FileHelper.UploadImage(request.FormFile);
            }
            else
            {
                category.Image = request.Image;
            }

            category.Type = request.Type;
            category.Status = request.Status;
            category.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return category;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Count()
    {
        return await _context.Categories.CountAsync();
    }

    public async Task<bool> Delete(List<int> ids)
    {
        try
        {
            var categoryIds = await _context.Categories.FirstOrDefaultAsync(c => ids.Contains(c.Id));

            if (categoryIds != null)
            {
                categoryIds.Status = StatusConstraint.DELETED;

                _context.Categories.Update(categoryIds);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Category not found");
            }

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}