using CookWizard.Domain.Users.Repository;
using MapsterMapper;

namespace CookWizard.Application.Users.UseCases.GetUserInfo;

public class GetUserInfoHandler
{
    public async Task<GetUserInfoResponse> Handle(
        GetUserInfoQuery query,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        var user = await userRepository.GetByIdAsync(query.Guid);
        if (user == null)
            throw new KeyNotFoundException("User doesnt found");
        return mapper.Map<GetUserInfoResponse>(user);
    }
}
