using CookWizard.Application.Users.Commands.Login;
using CookWizard.Domain.Auth.Repository;
using CookWizard.Domain.Auth.Services;

namespace CookWizard.Application.Auth.Login;

public class LoginHandler
{
    public async Task<LoginResponse> Handle(
        LoginCommand command,
        IUserCredentialsRepository userCredentialsRepository,
        IPasswordHasherService passwordHasher,
        IJWTService jwtService
    )
    {
        var existingCredentials = await userCredentialsRepository.GetCredentialsByEmail(command.Email);
        if (existingCredentials == null)
        {
            throw new Exception("User not found");
        }
        var password = Convert.FromBase64String(existingCredentials.PasswordHash);
        var salt = Convert.FromBase64String(existingCredentials.Salt);

        var credentialsMatch = passwordHasher.VerifyPassword(command.Password, salt, password);
        if (!credentialsMatch)
        {
            throw new Exception("Credentials not match");
        }

        var token = jwtService.GenerateToken(existingCredentials);

        return new LoginResponse(token);
    }
}
