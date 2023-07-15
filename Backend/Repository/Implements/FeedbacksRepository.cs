using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class FeedbacksRepository : IFeedbacksRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback> GetById(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);

            if (feedback == null)
            {
                throw new NotFoundException("Feedback not found.");
            }

            return feedback;
        }

        public async Task<List<Feedback>> GetByFilter(FilterModel filters)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.Name != null && f.Name.Contains(filters.Query ?? ""))
                .OrderBy(f => f.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return feedbacks;
        }

        public async Task<Feedback> Add(Feedback feedback)
        {
            try
            {
                feedback.CreateOn = DateTime.Now;
                await _context.Feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();

                return feedback;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Feedback> Update(int feedbackId, FeedbackRequest request)
        {
            try
            {
                var feedback = await GetById(feedbackId);

                feedback.Message = request.Message;
                feedback.Email = request.Email;
                feedback.Name = request.Name;
                await _context.SaveChangesAsync();

                return feedback;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.Feedbacks.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var feedbacks = await _context.Feedbacks.Where(f => ids.Contains(f.Id)).ToListAsync();
                _context.Feedbacks.RemoveRange(feedbacks);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
