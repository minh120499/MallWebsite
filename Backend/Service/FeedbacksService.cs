using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class FeedbacksService
    {
        private readonly IFeedbacksRepository _feedbacksRepository;

        public FeedbacksService(IFeedbacksRepository feedbacksRepository)
        {
            _feedbacksRepository = feedbacksRepository;
        }

        public async Task<Feedback> GetById(int feedbackId)
        {
            return await _feedbacksRepository.GetById(feedbackId);
        }

        public async Task<TableListResponse<Feedback>> GetByFilter(FilterModel filters)
        {
            var feedbacks = await _feedbacksRepository.GetByFilter(filters);
            var total = await _feedbacksRepository.Count();
            return new TableListResponse<Feedback>()
            {
                Total = total,
                Limit = filters.Limit,
                Page = filters.Page,
                Data = feedbacks
            };
        }

        public async Task<Feedback> Create(FeedbackRequest request)
        {
            Validations.Feedback(request);

            var feedback = new Feedback()
            {
                Name = request.Name,
                Email = request.Email,
                Message = request.Message,
            };
            return await _feedbacksRepository.Add(feedback);
        }

        public async Task<Feedback> Update(int feedbackId, FeedbackRequest request)
        {
            // Validations.Feedback(request);

            return await _feedbacksRepository.Update(feedbackId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var feedbackIds = ids.Split(',').Select(int.Parse).ToList();

            return await _feedbacksRepository.Delete(feedbackIds);
        }
    }
}