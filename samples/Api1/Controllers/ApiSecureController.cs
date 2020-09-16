using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [ApiController]
    [Route("invoices/[controller]")]
    public class ApiSecureController : ControllerBase
    {
        private readonly ILogger<ApiSecureController> logger;

        public ApiSecureController(ILogger<ApiSecureController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "InternalAnnouncementsDisplay invoices.read")]
        public async Task<ActionResult> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token"); //must be null
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            return Ok("Congrats you've reached method to read invoices!");
        }
    }
}
