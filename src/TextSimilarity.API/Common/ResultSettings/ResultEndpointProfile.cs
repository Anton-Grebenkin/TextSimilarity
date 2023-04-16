using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace TextSimilarity.API.Common.ResultSettings
{
    public class ResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
    {
        public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
        {
            var result = context.Result;

            if (result.HasError<UnauthorizedError>(out var unauthorizedErrors))
            {
                return new UnauthorizedObjectResult(unauthorizedErrors.First().Message);
            }


            if (result.HasError<NotFoundError>(out var notFoundErrors))
            {
                return new NotFoundObjectResult(notFoundErrors.First().Message);
            }

            if (result.HasError<Error>(out var errors))
            {
                return new BadRequestObjectResult(errors.First().Message);
            }

            return new StatusCodeResult(500);
        }
    }
}
