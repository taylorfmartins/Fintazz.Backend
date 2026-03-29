namespace Fintazz.Infrastructure.Email;

using Fintazz.Application.Abstractions.Services;

/// <summary>
/// Classe de jobs de e-mail executada pelo Hangfire Worker.
/// Cada método é um job independente — enfileirado pela API e processado pelo Worker.
/// </summary>
public class EmailJobs
{
    private readonly IEmailService _emailService;

    public EmailJobs(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendPasswordResetAsync(string toEmail, string toName, string resetToken)
    {
        await _emailService.SendPasswordResetAsync(toEmail, toName, resetToken);
    }

    public async Task SendEmailConfirmationAsync(string toEmail, string toName, string confirmationToken)
    {
        await _emailService.SendEmailConfirmationAsync(toEmail, toName, confirmationToken);
    }
}
