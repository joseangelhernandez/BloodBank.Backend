using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Exceptions;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Constants;
using BloodBank.Domain.Enums;

namespace BloodBank.Core.Features.Users.Commands;

public record UpdateMyProfileCommand(
    [Required, MaxLength(100)] string Name,
    [MaxLength(20)] string? Phone,
    Gender? Gender,
    [MaxLength(3)] string? BloodGroup,
    [MaxLength(150)] string? Location,
    [MaxLength(500)] string? PhotoUrl);

public class UpdateMyProfileHandler(IUserRepository users, IFirebaseAuthClient firebase)
{
    public async Task<UserDto> HandleAsync(string firebaseUid, string idToken, UpdateMyProfileCommand command, CancellationToken ct = default)
    {
        if (command.BloodGroup is not null && !BloodGroups.IsValid(command.BloodGroup))
            throw new BusinessException($"Blood group must be one of: {string.Join(", ", BloodGroups.All)}.");

        var user = await CurrentUser.GetAsync(users, firebaseUid, ct);

        await firebase.UpdateProfileAsync(idToken, command.Name.Trim(), command.PhotoUrl, ct);

        user.Name = command.Name.Trim();
        user.Phone = command.Phone;
        user.Gender = command.Gender;
        user.BloodGroup = command.BloodGroup;
        user.Location = command.Location;
        user.PhotoUrl = command.PhotoUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await users.UpdateAsync(user, ct);
        return UserDto.FromEntity(user);
    }
}
