using CookWizard.Application.Features.Users.Commands.Login;
using CookWizard.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[Route("api/[controller]")]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }
}