﻿using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            return product;
        }

        public async Task<List<Product>> GetByFilter(FilterModel filters)
        {
            var products = await _context.Products
                .Where(u => u.Name != null && u.Name.Contains(filters.Query ?? ""))
                .OrderBy(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
                .ToListAsync();
            return products;
        }

        public async Task<Product> Add(Product product)
        {
            try
            {
                product.CreateOn = DateTime.Now;
                product.ModifiedOn = DateTime.Now;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Product> Update(int productId, ProductRequest request)
        {
            try
            {
                var product = await GetById(productId);

                product.Code = request.Code;
                product.Image = request.Image;
                product.Name = request.Name;
                product.Description = request.Description;
                product.Brand = request.Brand;
                product.Categories = request.Categories;
                product.Variants = request.Variants;
                product.Status = request.Status;
                product.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> Count()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var products = await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
                _context.Products.RemoveRange(products);
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
