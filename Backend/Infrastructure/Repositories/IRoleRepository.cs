using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<List<Role>> GetAllAsync();
    Task<List<Role>> GetByFilterAsync(bool? isActive, string? searchTerm);
    Task<Role> CreateAsync(Role role);
    Task<Role> UpdateAsync(Role role);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
}
