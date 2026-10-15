using BloodBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Infrastructure.Persistence;

public class BloodBankDbContext(DbContextOptions<BloodBankDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<BloodRequest> BloodRequests => Set<BloodRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.FirebaseUid).HasMaxLength(128).IsRequired();
            user.HasIndex(u => u.FirebaseUid).IsUnique();

            user.Property(u => u.Name).HasMaxLength(100).IsRequired();
            user.Property(u => u.Email).HasMaxLength(150).IsRequired();
            user.HasIndex(u => u.Email);

            user.Property(u => u.Phone).HasMaxLength(20);
            user.Property(u => u.Gender).HasConversion<string>().HasMaxLength(10);
            user.Property(u => u.BloodGroup).HasMaxLength(3);
            user.Property(u => u.Location).HasMaxLength(150);
            user.Property(u => u.PhotoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<Donor>(donor =>
        {
            donor.Property(d => d.Name).HasMaxLength(100).IsRequired();
            donor.Property(d => d.ContactNumber).HasMaxLength(20).IsRequired();
            donor.Property(d => d.BloodGroup).HasMaxLength(3).IsRequired();
            donor.Property(d => d.Location).HasMaxLength(150).IsRequired();
            donor.Property(d => d.PhotoUrl).HasMaxLength(500);
            donor.HasIndex(d => d.BloodGroup);
            
            donor.HasOne(d => d.User)
                .WithOne(u => u.Donor)
                .HasForeignKey<Donor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BloodRequest>(request =>
        {
            request.Property(r => r.PatientName).HasMaxLength(100).IsRequired();
            request.Property(r => r.BloodGroup).HasMaxLength(3).IsRequired();
            request.Property(r => r.ContactNumber).HasMaxLength(20).IsRequired();
            request.Property(r => r.EmergencyContact).HasMaxLength(20).IsRequired();
            request.Property(r => r.Cause).HasMaxLength(150).IsRequired();
            request.Property(r => r.Gender).HasConversion<string>().HasMaxLength(10);
            request.Property(r => r.Location).HasMaxLength(150); // opcional
            request.Property(r => r.Notes).HasMaxLength(500);
            request.HasIndex(r => r.BloodGroup);
            request.HasIndex(r => r.CreatedAt);

            request.HasOne(r => r.Requester)
                .WithMany(u => u.BloodRequests)
                .HasForeignKey(r => r.RequesterId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<UtcDateTimeConverter>();
    }
}
