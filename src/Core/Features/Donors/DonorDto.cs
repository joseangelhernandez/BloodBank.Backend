using BloodBank.Domain.Entities;

namespace BloodBank.Core.Features.Donors;

public record DonorDto(
    Guid Id,
    Guid UserId,
    string Name,
    string ContactNumber,
    string BloodGroup,
    string Location,
    string? PhotoUrl,
    DateOnly LastDonationDate,
    bool IsEligible,
    DateTime CreatedAt)
{
    public static DonorDto FromEntity(Donor d) =>
        new(d.Id, d.UserId, d.Name, d.ContactNumber, d.BloodGroup, d.Location, d.PhotoUrl,
            d.LastDonationDate, d.IsEligibleToDonate(DateOnly.FromDateTime(DateTime.UtcNow)), d.CreatedAt);
}
