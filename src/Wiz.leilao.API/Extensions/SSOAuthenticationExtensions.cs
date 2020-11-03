using Microsoft.AspNetCore.Authentication;
using System;
using Wiz.leilao.API.Handler;

namespace Wiz.leilao.API.Extensions
{
    public static class SSOAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSSOAuthentication(
            this AuthenticationBuilder builder,
            Action<SSOAuthenticationOptions> configureOptions) =>
            builder.AddScheme<SSOAuthenticationOptions, SSOAuthenticationHandler>
                (SSOAuthenticationOptions.DefaultScheme, configureOptions);
    }
}
