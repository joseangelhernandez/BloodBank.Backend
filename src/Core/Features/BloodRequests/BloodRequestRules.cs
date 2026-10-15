using BloodBank.Core.Exceptions;
using BloodBank.Domain.Constants;

namespace BloodBank.Core.Features.BloodRequests;

public static class BloodRequestRules
{
    public static void Validate(string bloodGroup, DateOnly neededBefore)
    {
        if (!BloodGroups.IsValid(bloodGroup))
            throw new BusinessException($"Blood group must be one of: {string.Join(", ", BloodGroups.All)}.");

        if (neededBefore < DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)))
            throw new BusinessException("'Needed before' date cannot be in the past.");
    }
}
