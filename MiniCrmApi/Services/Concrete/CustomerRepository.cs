using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.DTOs.CustomerDtos;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Services.Concrete;

public class CustomerRepository(CrmDbContext _context) : ICustomerRepository
{
    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.Deals)
            .Include(c => c.Notes)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Deals)
            .Include(c => c.Notes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is not null)
        {
            customer.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}