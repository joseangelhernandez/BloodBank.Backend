namespace BloodBank.Domain.Constants;

public static class BloodGroups
{
    public static readonly IReadOnlyList<string> All =
        ["A+", "B+", "AB+", "O+", "A-", "B-", "AB-", "O-"];

    public static bool IsValid(string? value) => value is not null && All.Contains(value);
}
