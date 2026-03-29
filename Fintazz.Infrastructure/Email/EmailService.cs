namespace Fintazz.Infrastructure.Email;

using Fintazz.Application.Abstractions.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendPasswordResetAsync(string toEmail, string toName, string resetToken, CancellationToken cancellationToken = default)
    {
        var resetUrl = $"{_settings.AppBaseUrl}/reset-password?token={resetToken}";

        var body = $"""
            <p>Olá, <strong>{toName}</strong>!</p>
            <p>Recebemos uma solicitação para redefinir a senha da sua conta no Fintazz.</p>
            <p>Clique no link abaixo para criar uma nova senha. O link é válido por <strong>2 horas</strong>.</p>
            <p><a href="{resetUrl}">Redefinir Senha</a></p>
            <p>Se você não solicitou a redefinição de senha, ignore este e-mail.</p>
            """;

        await SendAsync(toEmail, toName, "Redefinição de Senha — Fintazz", body, cancellationToken);
    }

    public async Task SendEmailConfirmationAsync(string toEmail, string toName, string confirmationToken, CancellationToken cancellationToken = default)
    {
        var confirmUrl = $"{_settings.AppBaseUrl}/confirm-email?token={confirmationToken}";

        var body = $"""
            <p>Olá, <strong>{toName}</strong>!</p>
            <p>Obrigado por se cadastrar no Fintazz! Para ativar sua conta, confirme seu e-mail clicando no link abaixo.</p>
            <p>O link é válido por <strong>24 horas</strong>.</p>
            <p><a href="{confirmUrl}">Confirmar E-mail</a></p>
            """;

        await SendAsync(toEmail, toName, "Confirme seu e-mail — Fintazz", body, cancellationToken);
    }

    private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
