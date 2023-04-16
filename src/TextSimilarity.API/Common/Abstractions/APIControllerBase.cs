using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TextSimilarity.API.Common.Abstractions
{
    [ApiController]
    [Route("Api/[controller]")]
    public abstract class APIControllerBase : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
