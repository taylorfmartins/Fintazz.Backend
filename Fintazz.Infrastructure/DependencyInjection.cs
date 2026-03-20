namespace Fintazz.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Fintazz.Infrastructure.Auth;
using Fintazz.Infrastructure.Data;
using Fintazz.Infrastructure.Repositories;
using Fintazz.Domain.Repositories;
using Fintazz.Application.Abstractions.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<MongoContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHouseHoldRepository, HouseHoldRepository>();
        services.AddScoped<IHouseHoldInviteRepository, HouseHoldInviteRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<ICreditCardPurchaseRepository, CreditCardPurchaseRepository>();
        services.AddScoped<IRecurringChargeRepository, RecurringChargeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
