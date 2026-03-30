namespace Fintazz.Infrastructure.Auth;

public class AdminSettings
{
    public const string SectionName = "AdminSettings";
    public string[] AdminEmails { get; init; } = [];
}
