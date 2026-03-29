namespace Fintazz.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendPasswordResetAsync(string toEmail, string toName, string resetToken, CancellationToken cancellationToken = default);
    Task SendEmailConfirmationAsync(string toEmail, string toName, string confirmationToken, CancellationToken cancellationToken = default);
}
