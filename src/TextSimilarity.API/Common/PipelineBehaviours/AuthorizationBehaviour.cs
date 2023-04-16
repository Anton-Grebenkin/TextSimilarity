using FluentResults;
using MediatR;
using System.Reflection;
using TextSimilarity.API.Common.ResultSettings;
using TextSimilarity.API.Common.Security.Authorization;

namespace TextSimilarity.API.Common.PipelineBehaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
    {
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationBehaviour(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttribute = request.GetType().GetCustomAttributes<AuthorizeAttribute>().FirstOrDefault();
            if (authorizeAttribute != null)
            {
                var userInfo = _currentUserService.GetCurrentUser();
                if (userInfo?.UserId == null)
                {
                    return GetUnauthorizedResult();
                }

                if (userInfo.RequestSourse != authorizeAttribute.RequestSourse)
                {
                    return GetUnauthorizedResult();
                }
            }

            return await next();

        }

        private TResponse GetUnauthorizedResult()
        {
            var failResult = Result.Fail(Errors.UnauthorizedError("Unauthorized"));
            var result = new TResponse();
            foreach (var reason in failResult.Reasons)
            {
                result.Reasons.Add(reason);
            }
            return result;
        }
    }
}
