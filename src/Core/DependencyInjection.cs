using BloodBank.Core.Features.Auth.Commands;
using BloodBank.Core.Features.BloodRequests.Commands;
using BloodBank.Core.Features.BloodRequests.Queries;
using BloodBank.Core.Features.Donors.Commands;
using BloodBank.Core.Features.Donors.Queries;
using BloodBank.Core.Features.Users.Commands;
using BloodBank.Core.Features.Users.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace BloodBank.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<ForgotPasswordHandler>();

        services.AddScoped<GetMyProfileHandler>();
        services.AddScoped<UpdateMyProfileHandler>();

        services.AddScoped<GetBloodRequestsHandler>();
        services.AddScoped<GetBloodRequestByIdHandler>();
        services.AddScoped<CreateBloodRequestHandler>();
        services.AddScoped<UpdateBloodRequestHandler>();
        services.AddScoped<DeleteBloodRequestHandler>();

        services.AddScoped<GetDonorsHandler>();
        services.AddScoped<GetDonorByIdHandler>();
        services.AddScoped<GetMyDonorHandler>();
        services.AddScoped<RegisterDonorHandler>();
        services.AddScoped<UpdateDonorHandler>();
        services.AddScoped<DeleteDonorHandler>();

        return services;
    }
}
