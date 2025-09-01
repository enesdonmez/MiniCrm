using MiniCrmApi.Entities;

namespace MiniCrmApi.Services.Abstract;

public interface ICustomerNoteRepository
{
    Task<List<CustomerNote>> GetAllAsync();
    Task<CustomerNote?> GetByIdAsync(Guid id);
    Task AddAsync(CustomerNote customerNote);
    Task UpdateAsync(CustomerNote customerNote);
    Task DeleteAsync(Guid id);
}