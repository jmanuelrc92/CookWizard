using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces.Services;

public interface IJWTService
{
    string GenerateToken(User user);
}
