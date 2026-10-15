using BloodBank.Core.Interfaces;
using BloodBank.Infrastructure.Firebase;
using BloodBank.Infrastructure.Persistence;
using BloodBank.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BloodBank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BloodBankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BloodBank")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDonorRepository, DonorRepository>();
        services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();

        services.AddHttpClient<IFirebaseAuthClient, FirebaseAuthClient>();

        return services;
    }
}
