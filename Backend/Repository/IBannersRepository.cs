using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IBannersRepository
{
    Task<Banner> GetById(int bannerId);
    public Task<List<Banner>> GetByFilter(FilterModel filters);
    public Task<Banner> Add(Banner banner);
    public Task<Banner> Update(int bannerId, BannerRequest request);
    public Task<List<Banner>> UpdateByStore(List<Banner> banners, int storeId);
    public Task<int> Count();
    public Task<bool> Delete(List<int> ids);
}