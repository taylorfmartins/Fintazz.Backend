namespace Fintazz.Application.Exceptions;

using FluentValidation.Results;

public class ValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
