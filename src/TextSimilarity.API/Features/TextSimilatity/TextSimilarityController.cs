using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TextSimilarity.API.Common.Abstractions;
using TextSimilarity.API.Features.TextSimilatity.GetTextSimilarity.UseCase;

namespace TextSimilarity.API.Features.TextSimilatity
{
    public class TextSimilarityController : APIControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<GetTextSimilarityResponse>> TextSimilarity(GetTextSimilarityRequest request)
        {
            var result = await Mediator.Send(request);
            return result.ToActionResult();
        }
    }
}
