using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class OrderLineItemsRepository : IOrderLineItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderLineItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderLineItem> GetById(int orderLineItemId)
        {
            var orderLineItem = await _context.OrderLineItems.FindAsync(orderLineItemId);

            if (orderLineItem == null)
            {
                throw new NotFoundException("Order Line Item not found.");
            }

            return orderLineItem;
        }

        public async Task<List<OrderLineItem>> GetByFilter(FilterModel filters)
        {
            var orderLineItems = await _context.OrderLineItems
                .Where(u => u.ProductName != null && u.ProductName.Contains(filters.Query ?? ""))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return orderLineItems;
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

        public async Task<OrderLineItem> Update(int orderLineItemId, OrderLineItemRequest request)
        {
            try
            {
                var orderLineItem = await GetById(orderLineItemId);

                orderLineItem.OrderId = request.OrderId;
                orderLineItem.Order = request.Order;
                orderLineItem.ProductId = request.ProductId;
                orderLineItem.Product = request.Product;
                orderLineItem.ProductName = request.ProductName;
                orderLineItem.Variants = request.Variants;
                orderLineItem.Price = request.Price;
                orderLineItem.Quantity = request.Quantity;
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

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var orderLineItems = await _context.OrderLineItems.Where(o => ids.Contains(o.Id)).ToListAsync();
                _context.OrderLineItems.RemoveRange(orderLineItems);
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
