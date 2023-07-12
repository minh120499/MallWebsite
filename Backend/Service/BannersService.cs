using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class BannersService
{
    private readonly IBannersRepository _bannersRepository;

    public BannersService(IBannersRepository bannersRepository)
    {
        _bannersRepository = bannersRepository;
    }

    public async Task<TableListResponse<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _bannersRepository.GetByFilter(filters);
        var total = await _bannersRepository.Count();
        return new TableListResponse<Banner>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = banners
        };
    }

    public async Task<Banner> Create(BannerRequest request)
    {
        Validations.Banner(request);

        var banner = new Banner()
        {
            Name = request.Name,
            Image = request.Image,
            Status = StatusConstraint.ACTIVE,
        };
        return await _bannersRepository.Add(banner);
    }
}