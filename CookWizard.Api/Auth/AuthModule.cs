using Carter;
using CookWizard.Application.Auth.Login;
using CookWizard.Application.Users.Commands.Login;
using Wolverine;

namespace CookWizard.Api.Auth;

public class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api");
        authGroup.MapPost("/auth/token", Login);
    }

    private static async Task<IResult> Login(LoginCommand loginCommand, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<LoginResponse>(loginCommand);
        return Results.Ok(result);
    }
}
