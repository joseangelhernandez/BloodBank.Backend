using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Donors.Queries;

public record GetDonorsQuery(string? BloodGroup, string? Location);

public class GetDonorsHandler(IDonorRepository donors)
{
    public async Task<List<DonorDto>> HandleAsync(GetDonorsQuery query, CancellationToken ct = default)
    {
        var list = await donors.GetAllAsync(query.BloodGroup, query.Location, ct);
        return list.Select(DonorDto.FromEntity).ToList();
    }
}
