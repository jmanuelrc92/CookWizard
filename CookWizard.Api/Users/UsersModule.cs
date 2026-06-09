using Carter;
using CookWizard.Application.Users.CreateUser;
using Wolverine;

namespace CookWizard.Api.Users;

public class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var usersGroup = app.MapGroup("/api");

        usersGroup.MapPost("/users", CreateUser);
    }

    private static async Task<IResult> CreateUser(CreateUserCommand command, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<CreateUserResponse>(command);
        return Results.Ok(result);
    }
}
