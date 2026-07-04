using CookWizard.Domain.Auth.Models;
using CookWizard.Domain.Auth.Repository;
using CookWizard.Domain.Auth.Services;
using CookWizard.Domain.Users.Models;
using CookWizard.Domain.Users.Repository;

namespace CookWizard.Application.Users.UseCases.CreateUser;

public class CreateUserHandler
{
    public async Task<CreateUserResponse> Handle(
        CreateUserCommand command,
        IUserRepository userRepository,
        IUserCredentialsRepository userCredentialsRepository,
        IPasswordHasherService passwordHasher
    )
    {
        var exists = await userRepository.EmailExistAsync(command.Email);

        if (exists)
        {
            throw new Exception("User already exists");
        }

        var user = new User(
            command.Username,
            command.FirstName,
            command.LastName,
            command.Email,
            command.Birthday
        );

        await userRepository.AddAsync(user);

        var salt = passwordHasher.GenerateSalt(8);

        var hashedPassword = passwordHasher.HashPassword(
            command.Password,
            salt
        );

        var hashTmp = Convert.ToBase64String(hashedPassword);
        var saltTmp = Convert.ToBase64String(salt);

        var userCredential = new UserCredentials(
            user.Id,
            user.Email,
            hashTmp,
            saltTmp
        );

        await userCredentialsRepository.AddAsync(userCredential);

        return new CreateUserResponse(
            user.Id,
            user.Username,
            user.Email
        );

    }
}
