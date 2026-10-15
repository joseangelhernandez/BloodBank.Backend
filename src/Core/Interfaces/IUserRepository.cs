using BloodBank.Domain.Entities;

namespace BloodBank.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByFirebaseUidAsync(string firebaseUid, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
}
