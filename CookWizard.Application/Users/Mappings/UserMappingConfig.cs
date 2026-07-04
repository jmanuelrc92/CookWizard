using CookWizard.Application.Users.UseCases.GetUserInfo;
using CookWizard.Domain.Users.Models;
using Mapster;

namespace CookWizard.Application.Users.Mappings;

public static class UserMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, GetUserInfoResponse>
            .NewConfig()
            .Map(dest => dest.Guid, src => src.Id)
            .Map(dest => dest.MemberSince, src => src.CreatedAt);
    }
}
