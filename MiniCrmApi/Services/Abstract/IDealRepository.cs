using MiniCrmApi.Entities;

namespace MiniCrmApi.Services.Abstract;

public interface IDealRepository
{
    Task<List<Deal>> GetAllAsync();
    Task<Deal?> GetByIdAsync(Guid id);
    Task AddAsync(Deal deal);
    Task UpdateAsync(Deal deal);
    Task DeleteAsync(Guid id);
}