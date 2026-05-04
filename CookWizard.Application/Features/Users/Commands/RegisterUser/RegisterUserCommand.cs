using CookWizard.Application.Common;
using MediatR;

namespace CookWizard.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Username,
    string Password
) : IRequest<ResultObject<string>>;

