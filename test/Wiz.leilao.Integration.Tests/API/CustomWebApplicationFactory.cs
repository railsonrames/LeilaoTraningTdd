using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net.Http;
using Wiz.leilao.API.Services.Interfaces;
using Wiz.leilao.Domain.Interfaces.Identity;
using Wiz.leilao.Domain.Interfaces.Notifications;

namespace Wiz.leilao.Integration.Tests.API
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
     where TEntryPoint : class
    {
        public CustomWebApplicationFactory()
        {
            this.ClientOptions.AllowAutoRedirect = false;
            this.ClientOptions.BaseAddress = new Uri("https://localhost");
        }

        public Mock<ICustomerService> CustomerService { get; set; }
        public Mock<IProdutoService> ProdutoService { get; set; }

        public Mock<IIdentityService> IdentityService { get; set; }
        public Mock<IDomainNotification> DomainNotification { get; set; }

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                this.CustomerService = serviceProvider.GetRequiredService<Mock<ICustomerService>>();
                this.ProdutoService = serviceProvider.GetRequiredService<Mock<IProdutoService>>();

                this.DomainNotification = serviceProvider.GetRequiredService<Mock<IDomainNotification>>();
                this.IdentityService = serviceProvider.GetRequiredService<Mock<IIdentityService>>();
            }

            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder
                .UseEnvironment("Testing")
                .UseStartup<StartupTest>();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}
