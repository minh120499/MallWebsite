using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IFeedbacksRepository
{
    Task<Feedback> GetById(int feedbackId);
    Task<List<Feedback>> GetByFilter(FilterModel filters);
    Task<Feedback> Add(Feedback feedback);
    Task<Feedback> Update(int feedbackId, FeedbackRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}