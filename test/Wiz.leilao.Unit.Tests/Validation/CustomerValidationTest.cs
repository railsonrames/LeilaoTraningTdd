using FluentAssertions;
using Moq;
using Wiz.leilao.API.ViewModels.Customer;
using Wiz.leilao.Domain.Interfaces.Repository;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Domain.Models.Dapper;
using Wiz.leilao.Domain.Validation.CustomerValidation;
using Xunit;

namespace Wiz.Segmentacao.Unit.Tests.Validations
{
    public class CustomerValidationTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        private CustomerInsertValidation Validator { get; }
        
        private Customer entity;

        public CustomerValidationTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            CustomerAddress customerAddress = null;

            _customerRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(customerAddress);

            Validator = new CustomerInsertValidation(_customerRepositoryMock.Object);

            this.entity = new Customer(id: 1, addressId: 1, name: "fulano de tal");
        }

        [Fact]
        public void CustomerIsValid()
        {
            var validator = Validator.Validate(entity);
            validator.IsValid.Should().BeTrue();
        }

        [Fact]
        public void NotAllowEmptyName()
        {
            this.entity.Name = string.Empty;
            var validator = Validator.Validate(entity);

            validator.IsValid.Should().BeFalse();
        }
    }
}
