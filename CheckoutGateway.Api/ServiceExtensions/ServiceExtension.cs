using CheckoutGateway.BusinessLogic.Commands.RequestPayment;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Service;
using CheckoutGateway.BusinessLogic.Services.Caching;
using CheckoutGateway.DataLayer.Context;
using CheckoutGateway.DataLayer.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CheckoutGateway.Api.ServiceExtensions;

internal static class ServiceExtension
{
    public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
        services
            .AddScoped<IBankProxy, BankProxy>()
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<IValidator<RequestPaymentCommand>, RequestPaymentValidator>();

    public static IServiceCollection AddDatabaseService(this IServiceCollection service, IConfiguration configuration) => service.AddDbContext<DatabaseContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("SqlServer"), builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
        //options.UseOpenIddict();
    });
}
