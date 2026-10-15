using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.BloodRequests.Queries;

public record GetBloodRequestsQuery(string? BloodGroup);

public class GetBloodRequestsHandler(IBloodRequestRepository requests)
{
    public async Task<List<BloodRequestDto>> HandleAsync(GetBloodRequestsQuery query, CancellationToken ct = default)
    {
        var list = await requests.GetAllAsync(query.BloodGroup, ct);
        return list.Select(BloodRequestDto.FromEntity).ToList();
    }
}
