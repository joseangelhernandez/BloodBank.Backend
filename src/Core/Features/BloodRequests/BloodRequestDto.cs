using BloodBank.Domain.Entities;
using BloodBank.Domain.Enums;

namespace BloodBank.Core.Features.BloodRequests;

public record BloodRequestDto(
    Guid Id,
    Guid RequesterId,
    string PatientName,
    string BloodGroup,
    int Bags,
    string ContactNumber,
    string EmergencyContact,
    string Cause,
    Gender? Gender,
    string? Location,
    DateOnly NeededBefore,
    string? Notes,
    DateTime CreatedAt)
{
    public static BloodRequestDto FromEntity(BloodRequest r) =>
        new(r.Id, r.RequesterId, r.PatientName, r.BloodGroup, r.Bags, r.ContactNumber, r.EmergencyContact,
            r.Cause, r.Gender, r.Location, r.NeededBefore, r.Notes, r.CreatedAt);
}
