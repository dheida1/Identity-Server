using Api2.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("inventory/[controller]")]
    public class ApiDelegateController : ControllerBase
    {
        private readonly ILogger<ApiDelegateController> logger;
        private readonly IMediator mediator;

        public ApiDelegateController(
            ILogger<ApiDelegateController> logger,
            IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await mediator.Send(new Api3Request());
            return Ok(result);
        }
    }
}
