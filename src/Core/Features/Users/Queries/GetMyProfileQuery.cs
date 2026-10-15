using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Users.Queries;

public record GetMyProfileQuery(string FirebaseUid);

public class GetMyProfileHandler(IUserRepository users)
{
    public async Task<UserDto> HandleAsync(GetMyProfileQuery query, CancellationToken ct = default)
    {
        var user = await CurrentUser.GetAsync(users, query.FirebaseUid, ct);
        return UserDto.FromEntity(user);
    }
}
