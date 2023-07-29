using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class CategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository;

    public CategoriesService(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    
    public async Task<Category> GetById(int categoryId)
    {
        return await _categoriesRepository.GetById(categoryId);
    }

    public async Task<TableListResponse<Category>> GetByFilter(FilterModel filters)
    {
        var categories = await _categoriesRepository.GetByFilter(filters);
        var total = await _categoriesRepository.Count();
        return new TableListResponse<Category>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = categories
        };
    }

    public async Task<Category> Create(CategoryRequest request)
    {
        Validations.Category(request);
        var image = await FileHelper.UploadImage(request.FormFile);
        var category = new Category()
        {
            Name = request.Name,
            Image = image,
            Type = request.Type,
            Status = StatusConstraint.ACTIVE,
        };
        return await _categoriesRepository.Add(category);
    }

    public async Task<Category> Update(int categoryId, CategoryRequest request)
    {
        Validations.Category(request);

        return await _categoriesRepository.Update(categoryId, request);
    }

    public async Task<bool> Delete(string ids)
    {
        var categoriesIds = ids.Split(',').Select(int.Parse).ToList();

        return await _categoriesRepository.Delete(categoriesIds);
    }
}