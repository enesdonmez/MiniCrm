using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Services.Concrete;

public class DealRepository(CrmDbContext _context) : IDealRepository
{
    public async Task<List<Deal>> GetAllAsync()
    {
        return await _context.Deals.AsNoTracking().ToListAsync();
    }

    public async Task<Deal?> GetByIdAsync(Guid id)
    {
        return await _context.Deals.FindAsync(id);
    }

    public async Task AddAsync(Deal deal)
    {
        _context.Deals.Add(deal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Deal deal)
    {
        _context.Deals.Update(deal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var deal = await GetByIdAsync(id);
        if (deal is null) return;
        _context.Deals.Remove(deal);
        await _context.SaveChangesAsync();
    }
}