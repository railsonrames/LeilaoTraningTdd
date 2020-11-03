using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Wiz.leilao.API.Handler
{
    public class SSOAuthenticationHandler : AuthenticationHandler<SSOAuthenticationOptions>
    {
        private string _messageAuth;        

        public SSOAuthenticationHandler(
            IOptionsMonitor<SSOAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
       {
            _messageAuth = string.Empty;

            if (!Request.Headers.TryGetValue(Options.HeaderKey, out var ssoJWT))
            {
                return AuthenticateResult.NoResult();
            }

            var userIsRequired = UrlAllowAnonymous() == false;
            var jwtToken = new JwtSecurityToken(ssoJWT.FirstOrDefault());
            var userIdentification = jwtToken.Claims.FirstOrDefault(x => x.Type == "documento")?.Value;
            var clientId = jwtToken.Claims.FirstOrDefault(x => x.Type == "client_id")?.Value;
            var tenantId = jwtToken.Claims.FirstOrDefault(x => x.Type == "tenant")?.Value;

            if (string.IsNullOrEmpty(clientId))
            {
                _messageAuth = "ClientId not found";
                return AuthenticateResult.Fail(_messageAuth);
            }

            if (string.IsNullOrEmpty(userIdentification))
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    _messageAuth = "User identification not found in SSO";
                    return AuthenticateResult.Fail(_messageAuth);
                }
            }

            var claims = new List<Claim>
            {
                new Claim("GM-X-API-KEY", ssoJWT.FirstOrDefault()),
                new Claim("GM-USER-IDENTIFICATION", userIdentification ?? ""),
                new Claim("GM-USER-ID", "so-teste"),
                new Claim("GM-CLIENT-ID", clientId),
                new Claim("GM-TENANT-ID", clientId),
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, Options.Scheme));

            return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(claimsPrincipal), Options.Scheme));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.Headers.Add(Options.HeaderKey, Options.Scheme);

            if (!string.IsNullOrEmpty(_messageAuth))
            {
                var jsonString = "{\"Detail\": \"" + _messageAuth + "\"}";
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                Response.ContentType = "application/json";

                Response.Body.WriteAsync(data, 0, data.Length);
            }

            return Task.CompletedTask;
        }

        private bool UrlAllowAnonymous()
        {
            var anonymousUrls = new List<string>()
            {
                $"post/api/v1/users",
            };

            var currentUrl = $"{Request.Method}{Request.Path.Value}".ToLower();
            return anonymousUrls.Any(x => x == currentUrl);
        }

        private bool UrlRequiresAdm()
        {
            var allowedUrls = new List<string>()
            {
                $"post/api/v1/users",
                $"put/api/v1/users",
                $"post/api/v1/events",
                $"post/api/v1/events/action",
                $"post/api/v1/events/game",
                $"post/api/v1/events/synchronous",
                $"post/api/v1/events/game/finish",
            };

            var currentUrl = $"{Request.Method}{Request.Path.Value}".ToLower();

            if (currentUrl.Substring(currentUrl.Length-1) == "/"){
                currentUrl = currentUrl.Substring(0, currentUrl.Length - 1);
            }
            
            if (allowedUrls.Any(x => x == currentUrl)) {
                return false;
            }

            var controllerName = this.GetControllerName(currentUrl);

            if (Request.Method.ToLower() == "get" && controllerName != "reports")
            {
                return false;
            }

            return true;
        }

        private string GetControllerName(string url)
        {
            var vet = url.Split('/');
            if (vet.Length < 4)
                return string.Empty;

            return vet[3];
        }
    }
}
