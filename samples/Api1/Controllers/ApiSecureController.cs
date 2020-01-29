using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Api1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiSecureController : ControllerBase
    {
        private readonly ILogger<ApiSecureController> _logger;

        public ApiSecureController(ILogger<ApiSecureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(new JsonResult(from c in User.Claims select new { c.Type, c.Value }));
        }
    }
}
