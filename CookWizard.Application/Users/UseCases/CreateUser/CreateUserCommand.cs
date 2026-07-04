namespace CookWizard.Application.Users.UseCases.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    DateOnly Birthday
);