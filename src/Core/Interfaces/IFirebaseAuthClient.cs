namespace BloodBank.Core.Interfaces;

public record FirebaseSession(string Uid, string Email, string IdToken, int ExpiresIn);
public interface IFirebaseAuthClient
{
    Task<FirebaseSession> SignUpAsync(string email, string password, CancellationToken ct = default);
    Task<FirebaseSession> SignInAsync(string email, string password, CancellationToken ct = default);
    Task SendPasswordResetEmailAsync(string email, CancellationToken ct = default);

    Task UpdateProfileAsync(string idToken, string displayName, string? photoUrl, CancellationToken ct = default);
}
