using FluentValidation;
using MediatR;

namespace Matt.SharedKernel.Application.Validations;

public class ValidationBehavior<TRequest, TResponse>(
    IValidator<TRequest>? validator = null) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator == null)
        {
            return await next();
        }

        // before the handler
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        // after the handler
        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors.ToList();

        var errorMessages =
            errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToList()
                );

        throw new ValidationException(errorMessages);
    }
}

// public sealed class ValidationBehavior<TRequest, TResponse>(
//     IEnumerable<IValidator<TRequest>> validators)
//     : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     public async Task<TResponse> Handle(
//         TRequest request,
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         var context = new ValidationContext<TRequest>(request);
//
//         var validationFailures = await Task.WhenAll(
//             validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
//
//         var errors = validationFailures
//             .Where(validationResult => !validationResult.IsValid)
//             .SelectMany(validationResult => validationResult.Errors).ToList();
//
//         if (errors.Count != 0)
//         {
//             var errorMessages =
//                 errors
//                     .GroupBy(e => e.PropertyName)
//                     .ToDictionary(
//                         group => group.Key,
//                         group => group.Select(e => e.ErrorMessage).ToList()
//                     );
//             throw new ValidationException(errorMessages);
//         }
//
//         var response = await next();
//
//         return response;
//     }
// }