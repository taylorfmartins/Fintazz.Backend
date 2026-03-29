namespace Fintazz.Application.Abstractions.Services;

public interface IEmailQueue
{
    void EnqueuePasswordReset(string toEmail, string toName, string resetToken);
    void EnqueueEmailConfirmation(string toEmail, string toName, string confirmationToken);
}
