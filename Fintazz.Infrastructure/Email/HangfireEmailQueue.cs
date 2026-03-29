namespace Fintazz.Infrastructure.Email;

using Fintazz.Application.Abstractions.Services;
using Hangfire;

public class HangfireEmailQueue : IEmailQueue
{
    private readonly IBackgroundJobClient _jobClient;

    public HangfireEmailQueue(IBackgroundJobClient jobClient)
    {
        _jobClient = jobClient;
    }

    public void EnqueuePasswordReset(string toEmail, string toName, string resetToken)
    {
        _jobClient.Enqueue<EmailJobs>(job => job.SendPasswordResetAsync(toEmail, toName, resetToken));
    }

    public void EnqueueEmailConfirmation(string toEmail, string toName, string confirmationToken)
    {
        _jobClient.Enqueue<EmailJobs>(job => job.SendEmailConfirmationAsync(toEmail, toName, confirmationToken));
    }
}
