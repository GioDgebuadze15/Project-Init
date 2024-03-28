using FluentValidation.Results;
using ProjectInit.Shared.Exceptions;
using Wolverine.FluentValidation;

namespace ProjectInit.Application.Behaviors;

public class FluentValidationBehavior<T> : IFailureAction<T>
{
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
        => throw new FluentValidationException(
            [
                ..failures.Select(e => e.ErrorMessage)
            ]
        );
}