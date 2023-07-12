using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class CategoriesService
{
    private readonly ICategoriesRepository _bannersRepository;

    public CategoriesService(ICategoriesRepository bannersRepository)
    {
        _bannersRepository = bannersRepository;
    }

    public async Task<TableListResponse<Category>> GetByFilter(FilterModel filters)
    {
        var banners = await _bannersRepository.GetByFilter(filters);
        var total = await _bannersRepository.Count();
        return new TableListResponse<Category>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = banners
        };
    }

    public async Task<Category> Create(CategoryRequest request)
    {
        Validations.Category(request);

        var banner = new Category()
        {
            Name = request.Name,
            Image = request.Image,
            Status = StatusConstraint.ACTIVE,
        };
        return await _bannersRepository.Add(banner);
    }
}