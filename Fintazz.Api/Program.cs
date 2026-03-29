using System.Text;
using System.Text.Json.Serialization;
using Fintazz.Application;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure;
using Fintazz.Infrastructure.Auth;
using Fintazz.Infrastructure.Data;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Fintazz API", Version = "v1" });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    var appXmlFile = "Fintazz.Application.xml";
    var appXmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, appXmlFile);
    if (System.IO.File.Exists(appXmlPath)) c.IncludeXmlComments(appXmlPath);
});

// Registrar camadas da Clean Architecture
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Hangfire — apenas client (API enfileira; Worker processa)
builder.Services.AddHangfire((provider, config) =>
{
    var mongoSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    config.UseMongoStorage(
        mongoSettings.ConnectionString,
        "FintazzHangfire",
        new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        });
});

// Autenticação JWT
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Formatação Global de Erros de Requisição ou Exceções Internas
builder.Services.AddExceptionHandler<Fintazz.Api.Infrastructure.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Unifica os erros de Sintaxe (Model Binding) do MVC com a formatação do nosso GlobalExceptionHandler
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value != null && e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Messages = e.Value!.Errors.First().ErrorMessage
            }).ToList();

        var result = new
        {
            Error = "InvalidRequest",
            Message = "Os dados enviados possuem formato sintático inválido ou são incompatíveis.",
            ValidationErrors = errors
        };

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(result);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintazz API v1"));
    app.UseReDoc(c =>
    {
        c.DocumentTitle = "Fintazz API Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
        c.RoutePrefix = "api-docs";
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed de categorias de sistema na inicialização
using (var scope = app.Services.CreateScope())
{
    var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
    await categoryRepository.SeedSystemCategoriesAsync();
}

app.Run();
