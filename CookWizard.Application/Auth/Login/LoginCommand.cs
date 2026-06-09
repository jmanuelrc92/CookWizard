namespace CookWizard.Application.Users.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
);