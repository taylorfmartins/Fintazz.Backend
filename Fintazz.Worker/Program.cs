using Fintazz.Infrastructure;
using Fintazz.Infrastructure.Email;
using Fintazz.Application;
using Fintazz.Infrastructure.Data;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Options;
using Fintazz.Worker.Jobs;

var builder = Host.CreateApplicationBuilder(args);

// Infraestrutura compartilhada (MongoDB + Repositórios — usa seção MongoDbSettings do appsettings)
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR (casos de uso da Application)
builder.Services.AddApplication();

// Registrar os Jobs como serviços para injeção de dependência
builder.Services.AddScoped<ProcessRecurringChargesJob>();
builder.Services.AddScoped<EmailJobs>();

var host = builder.Build();

// Configurar Hangfire com MongoDB como storage (usando a mesma conexão via MongoDbSettings)
var mongoSettings = host.Services.GetRequiredService<IOptions<MongoDbSettings>>().Value;

GlobalConfiguration.Configuration.UseMongoStorage(
    mongoSettings.ConnectionString,
    "FintazzHangfire",   // banco separado para metadados do Hangfire
    new MongoStorageOptions
    {
        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new CollectionMongoBackupStrategy()
        },
        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
    }
);

// Iniciar Hangfire Server
var backgroundJobServer = new BackgroundJobServer(new BackgroundJobServerOptions
{
    WorkerCount = 1,
    ServerName = "Fintazz.Worker"
});

// Registrar o CronJob diário (todo dia à meia-noite e 1 minuto)
RecurringJob.AddOrUpdate<ProcessRecurringChargesJob>(
    "process-recurring-charges",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Daily(0, 1)
);

await host.RunAsync();
