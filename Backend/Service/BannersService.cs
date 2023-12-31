﻿using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Service;

public class BannersService
{
    private readonly IBannersRepository _bannersRepository;

    public BannersService(IBannersRepository bannersRepository)
    {
        _bannersRepository = bannersRepository;
    }

    public async Task<Banner> GetById(int bannerId)
    {
        return await _bannersRepository.GetById(bannerId);
    }

    public async Task<TableListResponse<Banner>> GetByFilter(FilterModel filters)
    {
        var banners = await _bannersRepository.GetByFilter(filters);
        return new TableListResponse<Banner>()
        {
            Total = banners.totalCount,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = banners.Item2
        };
    }

    public async Task<Banner> Create(BannerRequest request)
    {
        Validations.Banner(request);
        var image = await FileHelper.UploadImage(request.FormFile);
        
        var banner = new Banner()
        {
            Name = request.Name,
            StoreId = request.StoreId,
            Image = image,
            StartOn = request.StartOn,
            EndOn = request.EndOn,
            Status = StatusConstraint.ACTIVE,
        };
        return await _bannersRepository.Add(banner);
    }

    public async Task<Banner> Update(int bannerId, BannerRequest request)
    {
        Validations.Banner(request);

        return await _bannersRepository.Update(bannerId, request);
    }

    public async Task<bool> Delete(string ids)
    {
        var bannerIds = ids.Split(',').Select(int.Parse).ToList();

        return await _bannersRepository.Delete(bannerIds);
    }
}