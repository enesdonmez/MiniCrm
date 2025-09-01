using MiniCrmApi.Entities;

namespace MiniCrmApi.Services.Abstract;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}