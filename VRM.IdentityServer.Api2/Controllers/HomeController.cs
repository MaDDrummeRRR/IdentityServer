using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VRM.IdentityServer.Api2.Controllers
{
    [Route("api/[controller]")]
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
            return "This is secret string from ApiTwo";
        }

        [Route("/GetSecret")]
        [HttpGet]
        public async Task<IActionResult> SecretFromApiOneAsync()
        {
            // Create HttpClient, retrieve token from IdentityServer
            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocumentResponse = await serverClient.GetDiscoveryDocumentAsync("https://localhost:5001/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = "client_id",
                ClientSecret = "client_secret",
                Resource = { "ApiOne" },
                Scope = "read write delete"
            });

            // Create HttpClient, set header with received token, get secret from ApiOne
            var apiClient = httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetStringAsync("https://localhost:6001/Index/");

            return Ok( new 
            {
                AccessToken = tokenResponse.AccessToken,
                SecretMessage = response
            });
        }
    }
}
