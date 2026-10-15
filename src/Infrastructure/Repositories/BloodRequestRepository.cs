using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;
using BloodBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Infrastructure.Repositories;

public class BloodRequestRepository(BloodBankDbContext db) : IBloodRequestRepository
{
    public async Task<List<BloodRequest>> GetAllAsync(string? bloodGroup, CancellationToken ct = default)
    {
        var query = db.BloodRequests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(bloodGroup))
            query = query.Where(r => r.BloodGroup == bloodGroup);

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync(ct);
    }

    public async Task<BloodRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.BloodRequests.FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task AddAsync(BloodRequest request, CancellationToken ct = default)
    {
        db.BloodRequests.Add(request);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(BloodRequest request, CancellationToken ct = default)
    {
        db.BloodRequests.Update(request);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(BloodRequest request, CancellationToken ct = default)
    {
        db.BloodRequests.Remove(request);
        await db.SaveChangesAsync(ct);
    }
}
