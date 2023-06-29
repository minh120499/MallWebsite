﻿using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employees>> GetByFilter(FilterModel filters)
    {
        var a = await _context.Employees!
            .Where(u => u.FullName.Contains(filters.Query ?? ""))
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return a;
    }
}