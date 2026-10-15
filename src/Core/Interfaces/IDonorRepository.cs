using BloodBank.Domain.Entities;

namespace BloodBank.Core.Interfaces;

public interface IDonorRepository
{
    Task<List<Donor>> GetAllAsync(string? bloodGroup, string? location, CancellationToken ct = default);
    Task<Donor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Donor?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Donor donor, CancellationToken ct = default);
    Task UpdateAsync(Donor donor, CancellationToken ct = default);
    Task DeleteAsync(Donor donor, CancellationToken ct = default);
}
