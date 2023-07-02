using System.Collections;
using AutoMapper;
using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class BannersService
{
    private readonly IBannersRepository _bannersRepository;
    private readonly IMapper _mapper;

    public BannersService(IBannersRepository bannersRepository, IMapper mapper)
    {
        _bannersRepository = bannersRepository;
        _mapper = mapper;
    }

    public async Task<List<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _bannersRepository.GetByFilter(filters);
        return banners;
    }

    public async Task<Banner> Create(BannerRequest request)
    {
        Validations.Banner(request);

        var banner = _mapper.Map<Banner>(request);
        return await _bannersRepository.Add(banner);
    }
}