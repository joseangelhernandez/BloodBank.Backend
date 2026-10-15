using BloodBank.Domain.Entities;
using BloodBank.Domain.Enums;

namespace BloodBank.Core.Features.Users;

public record UserDto(
    Guid Id,
    string FirebaseUid,
    string Name,
    string Email,
    string? Phone,
    Gender? Gender,
    string? BloodGroup,
    string? Location,
    string? PhotoUrl,
    DateTime CreatedAt)
{
    public static UserDto FromEntity(User u) =>
        new(u.Id, u.FirebaseUid, u.Name, u.Email, u.Phone, u.Gender, u.BloodGroup, u.Location, u.PhotoUrl, u.CreatedAt);
}
