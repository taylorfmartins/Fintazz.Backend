namespace Fintazz.Api.Infrastructure;

using Microsoft.AspNetCore.Diagnostics;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        httpContext.Response.ContentType = "application/json";

        // Tratamento elegante do formato ruim lançado nos DTOs ou conversões
        if (exception is BadHttpRequestException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Error = "InvalidRequest",
                Message = "Os dados enviados possuem formato inválido ou são incompatíveis."
            }, cancellationToken);

            return true;
        }

        // Tratamento da nossa ValidationException do Pipeline do MediatR
        if (exception is Fintazz.Application.Exceptions.ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Error = "ValidationError",
                Message = "Um ou mais erros de validação ocorreram.",
                ValidationErrors = validationException.Errors.Select(e => new { Field = e.PropertyName, Messages = e.ErrorMessage })
            }, cancellationToken);

            return true;
        }

        // Tratamento Genérico para erros inesperados
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            Error = "InternalServerError",
            Message = "Um erro interno e inesperado ocorreu na API."
        }, cancellationToken);

        return true;
    }
}
