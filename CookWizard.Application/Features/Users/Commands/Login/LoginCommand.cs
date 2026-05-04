using CookWizard.Application.Common;
using MediatR;

namespace CookWizard.Application.Features.Users.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ResultObject<string>>;