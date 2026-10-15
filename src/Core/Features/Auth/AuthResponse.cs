using BloodBank.Core.Features.Users;

namespace BloodBank.Core.Features.Auth;

public record AuthResponse(string Token, int ExpiresIn, UserDto User);
