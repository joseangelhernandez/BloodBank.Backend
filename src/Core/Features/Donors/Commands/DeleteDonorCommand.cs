using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Donors.Commands;

public record DeleteDonorCommand(Guid Id, string FirebaseUid);

public class DeleteDonorHandler(IDonorRepository donors, IUserRepository users)
{
    public async Task HandleAsync(DeleteDonorCommand command, CancellationToken ct = default)
    {
        var user = await CurrentUser.GetAsync(users, command.FirebaseUid, ct);
        var donor = await donors.GetByIdAsync(command.Id, ct)
            ?? throw new NotFoundException("Donor not found.");

        if (donor.UserId != user.Id)
            throw new ForbiddenException("You can only delete your own donor profile.");

        await donors.DeleteAsync(donor, ct);
    }
}
