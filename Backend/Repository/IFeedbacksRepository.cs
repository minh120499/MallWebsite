using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IFeedbacksRepository
{
    public Task<List<Feedback>> GetByFilter(FilterModel filters);
    public Task<Feedback> Add(Feedback banner);
    public Task<int> Count();
}