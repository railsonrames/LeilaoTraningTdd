using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wiz.leilao.API;
using Wiz.leilao.API.Services.Interfaces;
using Wiz.leilao.API.ViewModels.Customer;
using Wiz.leilao.Integration.Tests.API;
using Wiz.leilao.Unit.Tests.Mocks;
using Xunit;

namespace Wiz.leilao.Integration.Tests.Controllers
{
    public class ProdutoControllerTest :  CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<IProdutoService> _produtoServiceMock;

        public ProdutoControllerTest()
        {
            _produtoServiceMock = new Mock<IProdutoService>();
            _httpClient = this.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjJBMTlCMDIzQkREQTAzRjQ3RjEzQTZENkFBRDUzQzMwMTgyMEIyMzgiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJLaG13STczYUFfUl9FNmJXcXRVOE1CZ2dzamcifQ.eyJuYmYiOjE2MDMzOTMwOTcsImV4cCI6MTYwMzM5NjY5NywiaXNzIjoiaHR0cHM6Ly9zc28taG1sLXdlYi5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3Nzby1obWwtd2ViLmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImFwaS13eDEiXSwiY2xpZW50X2lkIjoiV1gxVGVzdCIsInN1YiI6IjM2ZWMzMjUwLTAzYmMtNDc3NC05YzA4LThlOWI1OGE3YmJmOCIsImF1dGhfdGltZSI6MTYwMzM5MzA5NywiaWRwIjoiV2l6SUQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVGlhZ28gQnJpdG8iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0aWFnb2JyaXRvQHdpenNvbHVjb2VzLmNvbS5iciIsImRvY3VtZW50byI6IjcwNjM4MzczMTE1IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoid2l6Lndpei53aXppZC5hZG1pbi5mdWxsIiwid2l6IjpbIjQyMjc4NDczMDAwMTAzIiwiV0laIFNPTFXDh8OVRVMgRSBDT1JSRVRBR0VNIERFIFNFR1VST1MgUy5BIl0sImVudGVycHJpc2UiOiI4MzAzZTRiZS1iZWNhLTQxOGQtODE0Yi1lNTg5ZjEzZTlkNGMiLCJzY29wZSI6WyJhcGktd3gxLmdhbWlmaWNhY2FvIl0sImFtciI6WyJwd2QiXX0.CVNlX6Im5YEBrhmMQu9IvEXlQ3HgGiXxjM2kcKRitjBUizuouVoWsPljZTiQxMEeSSWFpj7fxPD-ctvpSH1VgRiDVLsXhhbI7cMoBAYhtLvmvLCP9hLGw3afwvwkue2JcRlZhUg41RdFkaDY1P4Q_Af8ySSPh-CXzj1RbY-CeIil3Po4wepgBiz2FTeB1VHujdnxf79OSKkRzYywFO7EjvxharlhrQFQ4L6RkxY-4d5q2j70zWbHtKq2QsrEjPwvb5Vvha8EX2QzkNErw06Sn6qDZ6bVZGBuAZiDIhxcaQJuEvFRbb9XsBWswWJVqh5YQVjjONpetZ9KFlQtf8uDuQ");

            _produtoServiceMock = this.ProdutoService;

            this.DomainNotification.Setup(x => x.HasNotifications)
                .Returns(false);

            this.IdentityService.Setup(x => x.GetScope())
                .Returns("Wx1");
        }

        [Fact]
        public async Task GetAll_SuccessTestAsync()
        {
            _produtoServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(ProdutoMock.ProdutoViewModelFaker.Generate(3));

            var response = await _httpClient.GetAsync("/api/v1/produtos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
