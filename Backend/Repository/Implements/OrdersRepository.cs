using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            return order;
        }

        public async Task<List<Order>> GetByFilter(FilterModel filters)
        {
            var orders = await _context.Orders
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return orders;
        }

        public async Task<Order> Add(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Order> Update(int orderId, OrderRequest request)
        {
            try
            {
                var order = await GetById(orderId);

                order.Source = request.Source;
                order.SaleBy = request.SaleBy;
                order.OrdersLineItems = request.OrdersLineItems;
                order.TotalPrice = request.TotalPrice;
                order.Status = request.Status;
                order.ModifiedOn = DateTime.Now;
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

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var orders = await _context.Orders.Where(o => ids.Contains(o.Id)).ToListAsync();
                _context.Orders.RemoveRange(orders);
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
