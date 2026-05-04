using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces.Repository;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<string> CreateAsync(User user);
}