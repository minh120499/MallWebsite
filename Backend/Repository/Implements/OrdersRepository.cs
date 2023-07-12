using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class OrdersRepository : IOrdersRepository
{
    private readonly ApplicationDbContext _context;

    public OrdersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetByFilter(FilterModel filters)
    {
        var orders = await _context.Orders
            // .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return orders;
    }

    public async Task<Order> Add(Order order)
    {
        try
        {
            order.CreateOn = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Count()
    {
        return await _context.Orders.CountAsync();
    }
}