using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.BloodRequests.Commands;

public record DeleteBloodRequestCommand(Guid Id, string FirebaseUid);

public class DeleteBloodRequestHandler(IBloodRequestRepository requests, IUserRepository users)
{
    public async Task HandleAsync(DeleteBloodRequestCommand command, CancellationToken ct = default)
    {
        var user = await CurrentUser.GetAsync(users, command.FirebaseUid, ct);
        var request = await requests.GetByIdAsync(command.Id, ct)
            ?? throw new NotFoundException("Blood request not found.");

        if (request.RequesterId != user.Id)
            throw new ForbiddenException("You can only delete your own requests.");

        await requests.DeleteAsync(request, ct);
    }
}
