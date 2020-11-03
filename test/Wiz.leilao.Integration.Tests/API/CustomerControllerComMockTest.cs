using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wiz.leilao.API;
using Wiz.leilao.API.Controllers;
using Wiz.leilao.API.Services.Interfaces;
using Wiz.leilao.API.ViewModels.Customer;
using Wiz.leilao.Integration.Tests.API;
using Wiz.leilao.Unit.Tests.Mocks;
using Xunit;

namespace Wiz.leilao.Integration.Tests.Controllers
{
    public class CustomerControllerComMockTest :  CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<ICustomerService> _customerServiceMock;

        public CustomerControllerComMockTest()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _httpClient = this.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjJBMTlCMDIzQkREQTAzRjQ3RjEzQTZENkFBRDUzQzMwMTgyMEIyMzgiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJLaG13STczYUFfUl9FNmJXcXRVOE1CZ2dzamcifQ.eyJuYmYiOjE2MDMzOTMwOTcsImV4cCI6MTYwMzM5NjY5NywiaXNzIjoiaHR0cHM6Ly9zc28taG1sLXdlYi5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3Nzby1obWwtd2ViLmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImFwaS13eDEiXSwiY2xpZW50X2lkIjoiV1gxVGVzdCIsInN1YiI6IjM2ZWMzMjUwLTAzYmMtNDc3NC05YzA4LThlOWI1OGE3YmJmOCIsImF1dGhfdGltZSI6MTYwMzM5MzA5NywiaWRwIjoiV2l6SUQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVGlhZ28gQnJpdG8iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0aWFnb2JyaXRvQHdpenNvbHVjb2VzLmNvbS5iciIsImRvY3VtZW50byI6IjcwNjM4MzczMTE1IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoid2l6Lndpei53aXppZC5hZG1pbi5mdWxsIiwid2l6IjpbIjQyMjc4NDczMDAwMTAzIiwiV0laIFNPTFXDh8OVRVMgRSBDT1JSRVRBR0VNIERFIFNFR1VST1MgUy5BIl0sImVudGVycHJpc2UiOiI4MzAzZTRiZS1iZWNhLTQxOGQtODE0Yi1lNTg5ZjEzZTlkNGMiLCJzY29wZSI6WyJhcGktd3gxLmdhbWlmaWNhY2FvIl0sImFtciI6WyJwd2QiXX0.CVNlX6Im5YEBrhmMQu9IvEXlQ3HgGiXxjM2kcKRitjBUizuouVoWsPljZTiQxMEeSSWFpj7fxPD-ctvpSH1VgRiDVLsXhhbI7cMoBAYhtLvmvLCP9hLGw3afwvwkue2JcRlZhUg41RdFkaDY1P4Q_Af8ySSPh-CXzj1RbY-CeIil3Po4wepgBiz2FTeB1VHujdnxf79OSKkRzYywFO7EjvxharlhrQFQ4L6RkxY-4d5q2j70zWbHtKq2QsrEjPwvb5Vvha8EX2QzkNErw06Sn6qDZ6bVZGBuAZiDIhxcaQJuEvFRbb9XsBWswWJVqh5YQVjjONpetZ9KFlQtf8uDuQ");

            _customerServiceMock = this.CustomerService;

            this.DomainNotification.Setup(x => x.HasNotifications)
                .Returns(false);

            this.IdentityService.Setup(x => x.GetScope())
                .Returns("Wx1");
        }

        [Fact]
        public async Task GetAll_SucessTestAsync()
        {
            _customerServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(CustomerMock.CustomerAddressViewModelFaker.Generate(3));

            var response = await _httpClient.GetAsync("/api/v1/customers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_SucessTestAsync()
        {
            ////cenario
            var customerId = CustomerMock.CustomerIdViewModelFaker.Generate();

            _customerServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<CustomerIdViewModel>()))
                .ReturnsAsync(CustomerMock.CustomerViewModelFaker.Generate());

            ////Execucao
            var response = await _httpClient.GetAsync($"/api/v1/customers/{customerId.Id}");

            ////Validacao
            var actionResult = Assert.IsType<ActionResult<CustomerViewModel>>(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_NotFoundTestAsync()
        {
            var customerId = CustomerMock.CustomerIdViewModelFaker.Generate();

            CustomerAddressViewModel customer = null;
            
            _customerServiceMock.Setup(x => x.GetAddressByIdAsync(It.IsAny<CustomerIdViewModel>()))
               .ReturnsAsync(customer);

            var response = await _httpClient.GetAsync($"/api/v1/customers/{customerId.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetByName_SucessTestAsync()
        {
            var customerName = CustomerMock.CustomerNameViewModelFaker.Generate();

            _customerServiceMock.Setup(x => x.GetAddressByNameAsync(customerName))
                .ReturnsAsync(CustomerMock.CustomerAddressViewModelFaker.Generate());

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = await customerController.GetByName(customerName);

            var actionResult = Assert.IsType<OkObjectResult>(customerService.Result);
            var actionValue = Assert.IsType<CustomerAddressViewModel>(actionResult.Value);

            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode);
        }

        [Fact]
        public void Post_SucessTestAsync()
        {
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _customerServiceMock.Setup(x => x.Add(customer))
                .Returns(CustomerMock.CustomerViewModelFaker.Generate());

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = customerController.PostCustomer(customer);

            var actionResult = Assert.IsType<CreatedResult>(customerService.Result);
            var actionValue = Assert.IsType<CustomerViewModel>(actionResult.Value);

            Assert.NotNull(actionValue);
            Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
        }

        [Fact]
        public void Post_FailTestAsync()
        {
            CustomerViewModel customer = null;

            _customerServiceMock.Setup(x => x.Add(customer))
                .Returns(CustomerMock.CustomerViewModelFaker.Generate());

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = customerController.PostCustomer(customer);

            var actionResult = Assert.IsType<NotFoundResult>(customerService.Result);

            Assert.Equal(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }

        [Fact]
        public async Task Put_BadRequestTestAsync()
        {
            var id = 1;
            CustomerViewModel customer = null;

            _customerServiceMock.Setup(x => x.Update(customer));

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = await customerController.PutCustomer(id, customer);

            var actionResult = Assert.IsType<BadRequestResult>(customerService);

            Assert.Equal(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [Fact]
        public async Task Put_NotFoundTestAsync()
        {
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _customerServiceMock.Setup(x => x.Update(customer));

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = await customerController.PutCustomer(customer.Id, customer);

            var actionResult = Assert.IsType<NotFoundResult>(customerService);

            Assert.Equal(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }

        [Fact]
        public async Task Delete_SucessTestAsync()
        {
            var customerId = CustomerMock.CustomerIdViewModelFaker.Generate();
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _customerServiceMock.Setup(x => x.GetByIdAsync(customerId))
                .ReturnsAsync(customer);

            _customerServiceMock.Setup(x => x.Remove(customer));

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = await customerController.DeleteCustomer(customerId);

            var actionResult = Assert.IsType<NoContentResult>(customerService);

            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
        }

        [Fact]
        public async Task Delete_NotFoundTestAsync()
        {
            var customerId = CustomerMock.CustomerIdViewModelFaker.Generate();

            var customerController = new CustomerController(_customerServiceMock.Object);
            var customerService = await customerController.DeleteCustomer(customerId);

            var actionResult = Assert.IsType<NotFoundResult>(customerService);

            Assert.Equal(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }
    }
}
