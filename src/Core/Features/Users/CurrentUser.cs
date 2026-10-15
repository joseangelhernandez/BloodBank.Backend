using BloodBank.Core.Exceptions;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;

namespace BloodBank.Core.Features.Users;

public static class CurrentUser
{
    public static async Task<User> GetAsync(IUserRepository users, string firebaseUid, CancellationToken ct)
    {
        var user = await users.GetByFirebaseUidAsync(firebaseUid, ct);

        if (user is null)
            throw new BusinessException("User profile not found. Please log in again.", 401);

        return user;
    }
}
