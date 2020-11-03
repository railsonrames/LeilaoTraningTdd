using Microsoft.AspNetCore.Authentication;

namespace Wiz.leilao.API.Handler
{
    public class SSOAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Basic";
        public const string DefaultHeaderApiKey = "x-api-key";
        public const string DefaultHeaderApiSubscription = "x-api-subscription";

        public string Scheme => DefaultScheme;
        public string HeaderKey { get; set; }
    }
}
