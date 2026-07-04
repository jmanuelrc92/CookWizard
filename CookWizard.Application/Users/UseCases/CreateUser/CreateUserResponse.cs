namespace CookWizard.Application.Users.UseCases.CreateUser;

public record CreateUserResponse(
    string Id,
    string Username,
    string Email
);