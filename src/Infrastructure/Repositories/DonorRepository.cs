using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;
using BloodBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Infrastructure.Repositories;

public class DonorRepository(BloodBankDbContext db) : IDonorRepository
{
    public async Task<List<Donor>> GetAllAsync(string? bloodGroup, string? location, CancellationToken ct = default)
    {
        var query = db.Donors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(bloodGroup))
            query = query.Where(d => d.BloodGroup == bloodGroup);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(d => d.Location.Contains(location));

        return await query.OrderBy(d => d.Name).ToListAsync(ct);
    }

    public async Task<Donor?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Donors.FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<Donor?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.Donors.FirstOrDefaultAsync(d => d.UserId == userId, ct);
    }

    public async Task AddAsync(Donor donor, CancellationToken ct = default)
    {
        db.Donors.Add(donor);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Donor donor, CancellationToken ct = default)
    {
        db.Donors.Update(donor);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Donor donor, CancellationToken ct = default)
    {
        db.Donors.Remove(donor);
        await db.SaveChangesAsync(ct);
    }
}
