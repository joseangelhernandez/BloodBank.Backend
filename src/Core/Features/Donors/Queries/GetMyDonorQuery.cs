using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Donors.Queries;

public record GetMyDonorQuery(string FirebaseUid);

public class GetMyDonorHandler(IDonorRepository donors, IUserRepository users)
{
    public async Task<DonorDto> HandleAsync(GetMyDonorQuery query, CancellationToken ct = default)
    {
        var user = await CurrentUser.GetAsync(users, query.FirebaseUid, ct);
        var donor = await donors.GetByUserIdAsync(user.Id, ct)
            ?? throw new NotFoundException("You are not registered as a donor.");

        return DonorDto.FromEntity(donor);
    }
}
