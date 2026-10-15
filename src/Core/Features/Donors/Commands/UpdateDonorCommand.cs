using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;

namespace BloodBank.Core.Features.Donors.Commands;

public record UpdateDonorCommand(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(20)] string ContactNumber,
    [Required, MaxLength(3)] string BloodGroup,
    [Required, MaxLength(150)] string Location,
    [MaxLength(500)] string? PhotoUrl,
    DateOnly LastDonationDate);

public class UpdateDonorHandler(IDonorRepository donors, IUserRepository users)
{
    public async Task<DonorDto> HandleAsync(Guid id, string firebaseUid, UpdateDonorCommand command, CancellationToken ct = default)
    {
        DonorRules.Validate(command.BloodGroup, command.LastDonationDate);

        var user = await CurrentUser.GetAsync(users, firebaseUid, ct);
        var donor = await donors.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Donor not found.");

        if (donor.UserId != user.Id)
            throw new ForbiddenException("You can only edit your own donor profile.");

        donor.Name = command.Name.Trim();
        donor.ContactNumber = command.ContactNumber.Trim();
        donor.BloodGroup = command.BloodGroup;
        donor.Location = command.Location.Trim();
        donor.PhotoUrl = command.PhotoUrl;
        donor.LastDonationDate = command.LastDonationDate;
        donor.UpdatedAt = DateTime.UtcNow;

        await donors.UpdateAsync(donor, ct);
        return DonorDto.FromEntity(donor);
    }
}
