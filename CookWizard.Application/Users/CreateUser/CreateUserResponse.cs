namespace CookWizard.Application.Users.CreateUser;

public record CreateUserResponse(
    string Id,
    string Username,
    string Email
);