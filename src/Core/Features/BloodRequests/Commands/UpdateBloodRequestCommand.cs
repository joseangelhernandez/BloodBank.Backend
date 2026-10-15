using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Enums;

namespace BloodBank.Core.Features.BloodRequests.Commands;

public record UpdateBloodRequestCommand(
    [Required, MaxLength(100)] string PatientName,
    [Required, MaxLength(3)] string BloodGroup,
    [Range(1, 10)] int Bags,
    [Required, MaxLength(20)] string ContactNumber,
    [Required, MaxLength(20)] string EmergencyContact,
    [Required, MaxLength(150)] string Cause,
    Gender? Gender,
    [Required, MaxLength(150)] string Location,
    DateOnly NeededBefore,
    [MaxLength(500)] string? Notes);

public class UpdateBloodRequestHandler(IBloodRequestRepository requests, IUserRepository users)
{
    public async Task<BloodRequestDto> HandleAsync(Guid id, string firebaseUid, UpdateBloodRequestCommand command, CancellationToken ct = default)
    {
        BloodRequestRules.Validate(command.BloodGroup, command.NeededBefore);

        var user = await CurrentUser.GetAsync(users, firebaseUid, ct);
        var request = await requests.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Blood request not found.");

        if (request.RequesterId != user.Id)
            throw new ForbiddenException("You can only edit your own requests.");

        request.PatientName = command.PatientName.Trim();
        request.BloodGroup = command.BloodGroup;
        request.Bags = command.Bags;
        request.ContactNumber = command.ContactNumber.Trim();
        request.EmergencyContact = command.EmergencyContact.Trim();
        request.Cause = command.Cause.Trim();
        request.Gender = command.Gender;
        request.Location = command.Location.Trim();
        request.NeededBefore = command.NeededBefore;
        request.Notes = command.Notes;
        request.UpdatedAt = DateTime.UtcNow;

        await requests.UpdateAsync(request, ct);
        return BloodRequestDto.FromEntity(request);
    }
}
