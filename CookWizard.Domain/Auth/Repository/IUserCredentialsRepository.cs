using CookWizard.Domain.Auth.Models;

namespace CookWizard.Domain.Auth.Repository;

public interface IUserCredentialsRepository
{
    Task AddAsync(UserCredentials userCredentials);
    Task<UserCredentials> GetCredentialsByEmail(string email);
}
