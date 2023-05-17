using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TextSimilarity.API.Common.Extensions;

namespace TextSimilarity.API.Common.ResultSettings
{
    public class ResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
    {
        public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
        {
            var result = context.Result;

            if (result.HasError<ValidationError>(out var validationErrors))
            {
                return new BadRequestObjectResult(new ApiError { Message = validationErrors.First().Message });
            }

            if (result.HasError<UnauthorizedError>(out var unauthorizedErrors))
            {
                return new UnauthorizedObjectResult(new ApiError { Message = unauthorizedErrors.First().Message });
            }


            if (result.HasError<NotFoundError>(out var notFoundErrors))
            {
                return new NotFoundObjectResult(new ApiError { Message = notFoundErrors.First().Message });
            }

            if (result.HasError<Error>(out var errors))
            {
                return new BadRequestObjectResult(new ApiError { Message = errors.First().Message});
            }

            return new StatusCodeResult(500);
        }

        public class ApiError
        {
            public string Message { get; set; }
        }
    }
}
