using CookWizard.Domain.Auth.Models;

namespace CookWizard.Domain.Auth.Services;

public interface IJWTService
{
    string GenerateToken(UserCredentials credentials);
}