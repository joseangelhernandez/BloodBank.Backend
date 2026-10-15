using BloodBank.Domain.Common;
using BloodBank.Domain.Enums;

namespace BloodBank.Domain.Entities;

public class BloodRequest : BaseEntity
{
    public Guid RequesterId { get; set; }
    public User Requester { get; set; } = null!;

    public string PatientName { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public int Bags { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public string EmergencyContact { get; set; } = string.Empty;
    public string Cause { get; set; } = string.Empty;
    public Gender? Gender { get; set; }

    public string Location { get; set; } = string.Empty;

    public DateOnly NeededBefore { get; set; }
    public string? Notes { get; set; }
}
