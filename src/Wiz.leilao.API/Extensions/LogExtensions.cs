using Microsoft.AspNetCore.Builder;
using Wiz.leilao.API.Middlewares;

namespace Wiz.leilao.API.Extensions
{
    public static class LogExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
