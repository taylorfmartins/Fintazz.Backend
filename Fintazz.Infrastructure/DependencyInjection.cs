namespace Fintazz.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Fintazz.Infrastructure.Data;
using Fintazz.Infrastructure.Repositories;
using Fintazz.Domain.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoContext>();
        services.AddScoped<IHouseHoldRepository, HouseHoldRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<ICreditCardPurchaseRepository, CreditCardPurchaseRepository>();
        services.AddScoped<IRecurringChargeRepository, RecurringChargeRepository>();
        services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
