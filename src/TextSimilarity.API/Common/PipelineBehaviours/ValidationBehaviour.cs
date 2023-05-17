using FluentValidation;
using MediatR;
using FluentResults;
using TextSimilarity.API.Features.TextSimilatity.GetTextSimilarity.UseCase;
using FluentValidation.Results;
using TextSimilarity.API.Common.ResultSettings;

namespace TextSimilarity.API.Common.PipelineBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new () 
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {

                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

                if (failures.Any())
                {
                    var result = new TResponse();

                    var failResult = ValidationFailuresToFailResult(failures);

                    foreach (var reason in failResult.Reasons)
                        result.Reasons.Add(reason);

                    return result;
                }
                    
            }

            return await next();
        }


        private Result ValidationFailuresToFailResult (IEnumerable<ValidationFailure> failures)
        {
            var errors = failures.Select(f => new ValidationError(f.PropertyName.ToLower(), f.ErrorMessage));
            return Result.Fail(errors);
        }

    }
}
