namespace ProjectInit.Shared.Exceptions;

public class FluentValidationException(
    IReadOnlyCollection<string> validationErrors
) : Exception("Validation errors have occured!")
{
    public IReadOnlyCollection<string> ValidationErrors { get; private init; } = validationErrors;
}