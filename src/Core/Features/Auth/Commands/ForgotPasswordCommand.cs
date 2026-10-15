using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Auth.Commands;

public record ForgotPasswordCommand([Required, EmailAddress] string Email);

public class ForgotPasswordHandler(IFirebaseAuthClient firebase)
{
    public async Task HandleAsync(ForgotPasswordCommand command, CancellationToken ct = default)
    {
        await firebase.SendPasswordResetEmailAsync(command.Email.Trim(), ct);
    }
}
