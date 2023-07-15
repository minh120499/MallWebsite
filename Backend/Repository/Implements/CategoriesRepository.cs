using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
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

    public async Task<List<Category>> GetByFilter(FilterModel filters)
    {
        var categories = await _context.Categories
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .Reverse()
            .ToListAsync();
        return categories;
    }

    public async Task<Category> Add(Category category)
    {
        try
        {
            category.CreateOn = DateTime.Now;
            category.ModifiedOn = DateTime.Now;
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
            category.Image = request.Image;
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
            var categoryIds = await _context.Categories.Where(b => ids.Contains(b.Id)).ToListAsync();
            _context.Categories.RemoveRange(categoryIds);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}