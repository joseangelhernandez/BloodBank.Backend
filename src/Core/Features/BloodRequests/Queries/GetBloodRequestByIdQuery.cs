using BloodBank.Core.Exceptions;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.BloodRequests.Queries;

public record GetBloodRequestByIdQuery(Guid Id);

public class GetBloodRequestByIdHandler(IBloodRequestRepository requests)
{
    public async Task<BloodRequestDto> HandleAsync(GetBloodRequestByIdQuery query, CancellationToken ct = default)
    {
        var request = await requests.GetByIdAsync(query.Id, ct)
            ?? throw new NotFoundException("Blood request not found.");

        return BloodRequestDto.FromEntity(request);
    }
}
