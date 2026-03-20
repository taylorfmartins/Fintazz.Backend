using System.Text;
using Fintazz.Application;
using Fintazz.Infrastructure;
using Fintazz.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
