namespace CookWizard.Application.Users.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    DateOnly Birthday
);