using System.Collections.Immutable;
using FluentValidation;
using MediatR;

namespace ProjectInit.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = new())
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults
            .SelectMany(e => e.Errors)
            .Where(f => f is not null)
            .ToImmutableArray();

        if (failures.Length != 0)
            throw new ValidationException(failures);

        return await next();
    }
}