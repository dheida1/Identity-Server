using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMtlsClient.Web.Models;
using MvcMtlsClient.Web.Requests;
using System;
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
        public IActionResult Secure()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Api1()
        {
            try
            {
                var apiResult = await mediator.Send(new Api1Request());
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        public async Task<IActionResult> Api2()
        {
            try
            {
                var apiResult = await mediator.Send(new Api2Request());
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        public async Task<IActionResult> Api2Delegated()
        {
            try
            {
                var apiResult = await mediator.Send(new Api2DelegateRequest());
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Logout()
        {
            var result = await HttpContext.AuthenticateAsync();
            var properties = result.Properties;
            var provider = properties.Items[".AuthScheme"];
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var url = Url.Action("Index", "Home");
            return SignOut(new AuthenticationProperties { RedirectUri = url }, provider);

            //return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
