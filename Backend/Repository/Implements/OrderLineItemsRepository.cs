using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class OrderLineItemsRepository : IOrderLineItemsRepository
{
    private readonly ApplicationDbContext _context;

    public OrderLineItemsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderLineItem>> GetByFilter(FilterModel filters)
    {
        var orders = await _context.OrderLineItems
            // .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return orders;
    }

    public async Task<OrderLineItem> Add(OrderLineItem orderLineItem)
    {
        try
        {
            await _context.OrderLineItems.AddAsync(orderLineItem);
            await _context.SaveChangesAsync();

            return orderLineItem;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Count()
    {
        return await _context.OrderLineItems.CountAsync();
    }
}