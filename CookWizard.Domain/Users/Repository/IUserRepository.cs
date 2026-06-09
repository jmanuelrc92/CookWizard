using CookWizard.Domain.Users.Models;

namespace CookWizard.Domain.Users.Repository;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<bool> EmailExistAsync(string email);
}
