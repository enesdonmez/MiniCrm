using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Services.Concrete;

public class CustomerNoteRepository(CrmDbContext _context) : ICustomerNoteRepository
{
    public async Task<List<CustomerNote>> GetAllAsync()
    {
        return await _context.CustomerNotes.AsNoTracking().ToListAsync();
    }

    public async Task<CustomerNote?> GetByIdAsync(Guid id)
    {
        var customerNote = await _context.CustomerNotes.FindAsync(id);
        return customerNote;
    }

    public async Task AddAsync(CustomerNote customerNote)
    {
        await _context.CustomerNotes.AddAsync(customerNote);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerNote customerNote)
    {
        _context.CustomerNotes.Update(customerNote);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customerNote = await _context.CustomerNotes.FindAsync(id);
        _context.CustomerNotes.Remove(customerNote);
        await _context.SaveChangesAsync();
    }
}