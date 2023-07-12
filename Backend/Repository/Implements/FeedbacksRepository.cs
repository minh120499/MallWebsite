using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class FeedbacksRepository : IFeedbacksRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbacksRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Feedback>> GetByFilter(FilterModel filters)
    {
        var feedbacks = await _context.Feedbacks
            .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
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

    public async Task<int> Count()
    {
        return await _context.Feedbacks.CountAsync();
    }
}