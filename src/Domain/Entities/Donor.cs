using BloodBank.Domain.Common;

namespace BloodBank.Domain.Entities;

public class Donor : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public DateOnly LastDonationDate { get; set; }

    public bool IsEligibleToDonate(DateOnly today)
    {
        return true;
    }
}
