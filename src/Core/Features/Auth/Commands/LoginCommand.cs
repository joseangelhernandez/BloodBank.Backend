using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;

namespace BloodBank.Core.Features.Auth.Commands;

public record LoginCommand(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public class LoginHandler(IFirebaseAuthClient firebase, IUserRepository users)
{
    public async Task<AuthResponse> HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        var session = await firebase.SignInAsync(command.Email.Trim(), command.Password, ct);

        var user = await users.GetByFirebaseUidAsync(session.Uid, ct);
        if (user is null)
        {
            user = new User
            {
                FirebaseUid = session.Uid,
                Name = session.Email.Split('@')[0],
                Email = session.Email
            };
            await users.AddAsync(user, ct);
        }

        return new AuthResponse(session.IdToken, session.ExpiresIn, UserDto.FromEntity(user));
    }
}
