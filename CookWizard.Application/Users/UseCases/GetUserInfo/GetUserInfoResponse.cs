namespace CookWizard.Application.Users.UseCases.GetUserInfo;

public record class GetUserInfoResponse(
    string Guid,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    DateOnly Birthday,
    DateTime MemberSince
);
