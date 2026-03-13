namespace Fintazz.Domain.Shared;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A permissão de valor nulo foi fornecida.");
}
