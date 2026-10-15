using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;
using BloodBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Infrastructure.Repositories;

public class UserRepository(BloodBankDbContext db) : IUserRepository
{
    public async Task<User?> GetByFirebaseUidAsync(string firebaseUid, CancellationToken ct = default)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }
}
