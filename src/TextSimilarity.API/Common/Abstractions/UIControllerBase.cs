using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TextSimilarity.API.Common.Abstractions
{
    [ApiController]
    [Route("[controller]")]
    public abstract class UIControllerBase : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
