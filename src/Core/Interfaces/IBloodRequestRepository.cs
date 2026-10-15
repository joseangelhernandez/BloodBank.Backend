using BloodBank.Domain.Entities;

namespace BloodBank.Core.Interfaces;

public interface IBloodRequestRepository
{
    Task<List<BloodRequest>> GetAllAsync(string? bloodGroup, CancellationToken ct = default);
    Task<BloodRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(BloodRequest request, CancellationToken ct = default);
    Task UpdateAsync(BloodRequest request, CancellationToken ct = default);
    Task DeleteAsync(BloodRequest request, CancellationToken ct = default);
}
