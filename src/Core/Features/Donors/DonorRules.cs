using BloodBank.Core.Exceptions;
using BloodBank.Domain.Constants;

namespace BloodBank.Core.Features.Donors;

public static class DonorRules
{
    public static void Validate(string bloodGroup, DateOnly lastDonationDate)
    {
        if (!BloodGroups.IsValid(bloodGroup))
            throw new BusinessException($"Blood group must be one of: {string.Join(", ", BloodGroups.All)}.");

        if (lastDonationDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new BusinessException("Last donation date cannot be in the future.");
    }
}
