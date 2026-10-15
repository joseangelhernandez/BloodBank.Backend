using BloodBank.Core.Exceptions;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Donors.Queries;

public record GetDonorByIdQuery(Guid Id);

public class GetDonorByIdHandler(IDonorRepository donors)
{
    public async Task<DonorDto> HandleAsync(GetDonorByIdQuery query, CancellationToken ct = default)
    {
        var donor = await donors.GetByIdAsync(query.Id, ct)
            ?? throw new NotFoundException("Donor not found.");

        return DonorDto.FromEntity(donor);
    }
}
