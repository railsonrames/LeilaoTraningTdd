using AutoMapper;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiz.leilao.API.Services;
using Wiz.leilao.API.ViewModels.Customer;
using Wiz.leilao.Domain.Interfaces.Notifications;
using Wiz.leilao.Domain.Interfaces.Repository;
using Wiz.leilao.Domain.Interfaces.Services;
using Wiz.leilao.Domain.Interfaces.UoW;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Domain.Models.Dapper;
using Wiz.leilao.Domain.Validation.CustomerValidation;
using Wiz.leilao.Unit.Tests.Mocks;
using Xunit;

namespace Wiz.leilao.Unit.Tests.Services
{
    public class CustomerServiceTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICustomerInsertValidation> _customerInsertValidationMock;
        private readonly Mock<IViaCEPService> _viaCEPServiceMock;
        private readonly Mock<IDomainNotification> _domainNotificationMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public CustomerServiceTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerInsertValidationMock = new Mock<ICustomerInsertValidation>();
            _viaCEPServiceMock = new Mock<IViaCEPService>();
            _domainNotificationMock = new Mock<IDomainNotification>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAll_ReturnCustomerAddressViewModelTestAsync()
        {
            var cep = "17052520";

            _customerRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(CustomerMock.CustomerAddressModelFaker.Generate(3));

            _mapperMock.Setup(x => x.Map<IEnumerable<CustomerAddressViewModel>>(It.IsAny<IEnumerable<CustomerAddress>>()))
                .Returns(CustomerMock.CustomerAddressViewModelFaker.Generate(3));

            _viaCEPServiceMock.Setup(x => x.GetByCEPAsync(cep))
                .ReturnsAsync(ViaCEPMock.ViaCEPModelFaker.Generate());

            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, _domainNotificationMock.Object,
                _unitOfWorkMock.Object, _mapperMock.Object);

            var customeMethod = await customerService.GetAllAsync();

            var customerResult = Assert.IsAssignableFrom<IEnumerable<CustomerAddressViewModel>>(customeMethod);

            Assert.NotNull(customerResult);
            Assert.NotEmpty(customerResult);
        }

        [Fact]
        public async Task GetById_ReturnCustomerViewModelTestAsync()
        {
            int id = 1;
            var customerId = CustomerMock.CustomerIdViewModelFaker.Generate();

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(CustomerMock.CustomerModelFaker.Generate());

            _mapperMock.Setup(x => x.Map<CustomerViewModel>(It.IsAny<Customer>()))
                .Returns(CustomerMock.CustomerViewModelFaker.Generate());

            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, 
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object, _mapperMock.Object);

            var customeMethod = await customerService.GetByIdAsync(customerId);

            var customerResult = Assert.IsAssignableFrom<CustomerViewModel>(customeMethod);

            Assert.NotNull(customerResult);
        }

        [Fact]
        public async Task GetAddressByNameAsync_ReturnCustomerAddressViewModelTestAsync()
        {
            var name = "Diuor PleaBolosmakh";
            var customerName = CustomerMock.CustomerNameViewModelFaker.Generate();

            _customerRepositoryMock.Setup(x => x.GetByNameAsync(name))
                .ReturnsAsync(CustomerMock.CustomerAddressModelFaker.Generate());

            _mapperMock.Setup(x => x.Map<CustomerAddressViewModel>(It.IsAny<CustomerAddress>()))
                .Returns(CustomerMock.CustomerAddressViewModelFaker.Generate());

            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, 
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object, 
                _mapperMock.Object);

            var customeMethod = await customerService.GetAddressByNameAsync(customerName);

            var customerResult = Assert.IsAssignableFrom<CustomerAddressViewModel>(customeMethod);

            Assert.NotNull(customerResult);
        }

        [Fact]
        public void Add_ReturnCustomerViewModelTestAsync()
        {
            ////cenario
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _mapperMock.Setup(x => x.Map<Customer>(It.IsAny<CustomerViewModel>()))
                .Returns(CustomerMock.CustomerModelFaker.Generate());

            _mapperMock.Setup(x => x.Map<CustomerViewModel>(It.IsAny<Customer>()))
                .Returns(CustomerMock.CustomerViewModelFaker.Generate());

            _customerRepositoryMock.Setup(x => x.GetByNameAsync(customer.Name))
                .ReturnsAsync(CustomerMock.CustomerAddressModelFaker.Generate());

            var validateReturn = new ValidationResult();

            _customerInsertValidationMock.Setup(x => x.Validate(It.IsAny<Customer>()))
                .Returns(validateReturn);

            ////execucao
            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, 
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object, 
                _mapperMock.Object);

            customerService.Add(customer);

            ////Validacao
            Assert.NotNull(customer);

            _customerRepositoryMock.Verify(
                x => x.Add(It.IsAny<Customer>()), 
                Times.Once);

            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void Update_SucessTestAsync()
        {
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _mapperMock.Setup(x => x.Map<Customer>(It.IsAny<CustomerViewModel>()))
                .Returns(CustomerMock.CustomerModelFaker.Generate());

            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, 
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object, 
                _mapperMock.Object);

            customerService.Update(customer);

            Assert.NotNull(customer);
        }

        [Fact]
        public void Remove_SucessTestAsync()
        {
            var customer = CustomerMock.CustomerViewModelFaker.Generate();

            _mapperMock.Setup(x => x.Map<Customer>(It.IsAny<CustomerViewModel>()))
                .Returns(CustomerMock.CustomerModelFaker.Generate());

            var customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _customerInsertValidationMock.Object,
                _viaCEPServiceMock.Object, 
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object, 
                _mapperMock.Object);

            customerService.Remove(customer);

            Assert.NotNull(customer);
        }
    }
}
