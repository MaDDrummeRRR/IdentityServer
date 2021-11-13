using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VRM.IdentityServer.Api1.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [Route("/Index")]
        [HttpGet]
        [Authorize]
        public string Index()
        {
            return "This is secret string from ApiOne";
        }
    }
}
