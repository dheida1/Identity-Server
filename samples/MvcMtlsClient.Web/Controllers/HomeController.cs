using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMtlsClient.Web.Models;
using MvcMtlsClient.Web.Requests;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger,
             IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secure()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token"); //must be null
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Api1()
        {
            var apiResult = await mediator.Send(new Api1Request());
            return Ok(apiResult);
        }

        [Authorize]
        public async Task<IActionResult> Api2()
        {
            var apiResult = await mediator.Send(new Api2Request());
            return Ok(apiResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
