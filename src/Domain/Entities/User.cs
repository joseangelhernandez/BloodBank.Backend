using BloodBank.Domain.Common;
using BloodBank.Domain.Enums;

namespace BloodBank.Domain.Entities;

public class User : BaseEntity
{
    public string FirebaseUid { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public Gender? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Location { get; set; }
    public string? PhotoUrl { get; set; }

    public Donor? Donor { get; set; }
    public ICollection<BloodRequest> BloodRequests { get; set; } = [];
}
