using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IO.Compression;
using Wiz.leilao.API;
using Wiz.leilao.API.Extensions;
using Wiz.leilao.API.Filters;
using Wiz.leilao.API.Handler;
using Wiz.leilao.API.Services.Interfaces;
using Wiz.leilao.API.Settings;
using Wiz.leilao.Domain.Interfaces.Identity;
using Wiz.leilao.Domain.Interfaces.Notifications;
using Wiz.leilao.Infra.Context;

namespace Wiz.leilao.Integration.Tests.API
{
    /// <summary>
    /// Vamos mockar as dependencias dos serviços para testes de integração enxutos
    /// </summary>
    public class StartupTest : Startup
    {
        private Mock<ICustomerService> _customerService;
        private Mock<IProdutoService> _produtoService;
        private Mock<IDomainNotification> _domainNotification;
        private Mock<IIdentityService> _identityService;

        public StartupTest(
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment) :
                base(configuration, webHostEnvironment)
        {
            _customerService = new Mock<ICustomerService>(MockBehavior.Strict);
            _produtoService = new Mock<IProdutoService>(MockBehavior.Strict);
            _domainNotification = new Mock<IDomainNotification>(MockBehavior.Strict);
            _identityService = new Mock<IIdentityService>(MockBehavior.Strict);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddSingleton(this._customerService);
            services.AddSingleton(this._produtoService);

            services.AddSingleton(this._domainNotification);
            services.AddSingleton(this._identityService);
            
            services.AddAuthorization();
            services.AddControllers();

            services.AddMvc(options =>
            {
                options.Filters.Add<DomainNotificationFilter>();
                options.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = SSOAuthenticationOptions.DefaultScheme;
            })
            .AddSSOAuthentication(options =>
            {
                options.HeaderKey = SSOAuthenticationOptions.DefaultHeaderApiKey;
            });

            ////services.AddAuthorization(options =>
            ////{
            ////    options.AddPolicy("ProfileRequirement", policy =>
            ////        policy.Requirements.Add(new ProfileAuthorizationRequirement("gameId", "teamId")));
            ////});

            services.Configure<GzipCompressionProviderOptions>(x => x.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(x =>
            {
                x.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<ApplicationInsightsSettings>(configuracao =>
            {
                configuracao.InstrumentationKey = "abcd";
            });

            services.AddSingleton(this._customerService.Object);
            services.AddSingleton(this._produtoService.Object);

            services.AddSingleton(this._domainNotification.Object);
            services.AddSingleton(this._identityService.Object);
        }
    }
}
