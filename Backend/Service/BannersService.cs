﻿using Backend.Model;
using Backend.Model.Entities;
using Backend.Repository;

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

    public async Task<Banner> Create(Banner banner)
    {
        return await _bannersRepository.Add(banner);
    }
}