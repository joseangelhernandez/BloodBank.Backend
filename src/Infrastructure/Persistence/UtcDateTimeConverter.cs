using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BloodBank.Infrastructure.Persistence;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    toDb => toDb.ToUniversalTime(),
    fromDb => DateTime.SpecifyKind(fromDb, DateTimeKind.Utc));
