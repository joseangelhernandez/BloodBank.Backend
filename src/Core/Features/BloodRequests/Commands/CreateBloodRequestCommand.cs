using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;
using BloodBank.Domain.Enums;

namespace BloodBank.Core.Features.BloodRequests.Commands;

public record CreateBloodRequestCommand(
    [Required, MaxLength(100)] string PatientName,
    [Required, MaxLength(3)] string BloodGroup,
    [Range(1, 10)] int Bags,
    [Required, MaxLength(20)] string ContactNumber,
    [Required, MaxLength(20)] string EmergencyContact,
    [Required, MaxLength(150)] string Cause,
    Gender? Gender,
    [MaxLength(150)] string? Location,
    DateOnly NeededBefore,
    [MaxLength(500)] string? Notes);

public class CreateBloodRequestHandler(IBloodRequestRepository requests, IUserRepository users)
{
    public async Task<BloodRequestDto> HandleAsync(string firebaseUid, CreateBloodRequestCommand command, CancellationToken ct = default)
    {
        BloodRequestRules.Validate(command.BloodGroup, command.NeededBefore);

        var user = await CurrentUser.GetAsync(users, firebaseUid, ct);

        var request = new BloodRequest
        {
            RequesterId = user.Id,
            PatientName = command.PatientName.Trim(),
            BloodGroup = command.BloodGroup,
            Bags = command.Bags,
            ContactNumber = command.ContactNumber.Trim(),
            EmergencyContact = command.EmergencyContact.Trim(),
            Cause = command.Cause.Trim(),
            Gender = command.Gender,
            Location = command.Location?.Trim(),
            NeededBefore = command.NeededBefore,
            Notes = command.Notes
        };

        await requests.AddAsync(request, ct);
        return BloodRequestDto.FromEntity(request);
    }
}
