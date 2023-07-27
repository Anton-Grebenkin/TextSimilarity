using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TextSimilarity.API.Common.Abstractions;
using TextSimilarity.API.Features.TextSimilatity.GetTextSimilarity.UseCase;

namespace TextSimilarity.API.Features.TextSimilatity
{
    public class TextSimilarityController : APIControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<GetTextSimilarityResponse>> TextSimilarity(GetTextSimilarityRequest request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(request, cancellationToken);
            return result.ToActionResult();
        }
    }
}
