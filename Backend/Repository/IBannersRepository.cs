using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IBannersRepository
{
    public Task<List<Banner>> GetByFilter(FilterModel filters);
    public Task<Banner> Add(Banner banner);
}