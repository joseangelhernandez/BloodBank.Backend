using System.ComponentModel.DataAnnotations;
using BloodBank.Core.Exceptions;
using BloodBank.Core.Features.Users;
using BloodBank.Core.Interfaces;
using BloodBank.Domain.Entities;

namespace BloodBank.Core.Features.Donors.Commands;

public record RegisterDonorCommand(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(20)] string ContactNumber,
    [Required, MaxLength(3)] string BloodGroup,
    [Required, MaxLength(150)] string Location,
    [MaxLength(500)] string? PhotoUrl,
    DateOnly LastDonationDate);

public class RegisterDonorHandler(IDonorRepository donors, IUserRepository users)
{
    public async Task<DonorDto> HandleAsync(string firebaseUid, RegisterDonorCommand command, CancellationToken ct = default)
    {
        DonorRules.Validate(command.BloodGroup, command.LastDonationDate);

        var user = await CurrentUser.GetAsync(users, firebaseUid, ct);

        if (await donors.GetByUserIdAsync(user.Id, ct) is not null)
            throw new BusinessException("You are already registered as a donor.", 409);

        var donor = new Donor
        {
            UserId = user.Id,
            Name = command.Name.Trim(),
            ContactNumber = command.ContactNumber.Trim(),
            BloodGroup = command.BloodGroup,
            Location = command.Location.Trim(),
            PhotoUrl = command.PhotoUrl,
            LastDonationDate = command.LastDonationDate
        };

        await donors.AddAsync(donor, ct);
        return DonorDto.FromEntity(donor);
    }
}
