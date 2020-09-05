using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("inventory/[controller]")]
    public class ApiSecureController : ControllerBase
    {
        private readonly ILogger<ApiSecureController> logger;

        public ApiSecureController(ILogger<ApiSecureController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Policy = "myPolicy")]
        public async Task<ActionResult> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token"); //must be null
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            return Ok(new JsonResult(from c in User.Claims select new { c.Type, c.Value }));
        }
    }
}
