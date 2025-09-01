using MiniCrmApi.DTOs.CustomerDtos;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Services.Abstract;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
}