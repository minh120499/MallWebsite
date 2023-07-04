using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
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

    public async Task<List<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _bannersRepository.GetByFilter(filters);
        return banners;
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