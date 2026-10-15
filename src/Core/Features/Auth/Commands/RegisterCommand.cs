using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;

namespace BloodBank.Core.Features.Auth.Commands;

public record RegisterCommand(
    [Required, MaxLength(100)] string Name,
    [Required, EmailAddress, MaxLength(150)] string Email,
    [Required] string Password);

public class RegisterHandler(IFirebaseAuthClient firebase, IUserRepository users)
{
    public async Task<AuthResponse> HandleAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var session = await firebase.SignUpAsync(command.Email.Trim(), command.Password, ct);
        var user = new User
        {
            FirebaseUid = session.Uid,
            Name = command.Name.Trim(),
            Email = session.Email
        };
        await users.AddAsync(user, ct);

        return new AuthResponse(session.IdToken, session.ExpiresIn, UserDto.FromEntity(user));
    }
}
