using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetByFilterAsync(bool? isActive, string? searchTerm);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}


