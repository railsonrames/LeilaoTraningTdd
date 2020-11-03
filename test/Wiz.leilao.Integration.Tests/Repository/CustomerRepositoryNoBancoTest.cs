using Bogus.DataSets;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Infra.Context;
using Wiz.leilao.Infra.Repository;
using Wiz.leilao.Infra.UoW;
using Wiz.leilao.Integration.Tests.Fixtures;
using Wiz.leilao.Unit.Tests.Mocks;
using Xunit;

namespace Wiz.leilao.Integration.Tests.Repository
{
    [Collection("DatabaseTestCollection")]
    public class CustomerRepositoryNoBancoTest : IDisposable
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly DbContextOptions<EntityContext> _entityOptions;
        private readonly DatabaseFixture _databaseFixture;

        public CustomerRepositoryNoBancoTest(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;

            _configurationMock = new Mock<IConfiguration>();
            _entityOptions = new DbContextOptionsBuilder<EntityContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Crud_EntityTest()
        {
            var customer = CustomerMock.CustomerModelFaker.Generate();

            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var entityContext = new EntityContext(_entityOptions);
            var unitOfWork = new UnitOfWork(entityContext);
            var dapperContext = new DapperContext(_configurationMock.Object);
            var customerRepository = new CustomerRepository(entityContext, dapperContext);

            customerRepository.Add(customer);
            var IsSaveCustomer = unitOfWork.Commit();

            customerRepository.Update(customer);
            var IsUpdateCustomer = unitOfWork.Commit();

            customerRepository.Remove(customer);
            var IsRemoverCustomer = unitOfWork.Commit();

            Assert.Equal(1, IsSaveCustomer);
            Assert.Equal(1, IsUpdateCustomer);
            Assert.Equal(1, IsRemoverCustomer);
        }

        [Fact]
        public void DeveRetornarEnderecoPorId()
        {
            ///cenario
            var address = new Wiz.leilao.Domain.Models.Address(cep: "72110380");
            address.Persist(_databaseFixture.Context);

            var customer = CustomerMock.CustomerModelFaker.Generate();
            customer.Id = 0;
            customer.AddressId = address.Id;

            customer.Persist(_databaseFixture.Context);

            ////execução
            var customerAdress = this._databaseFixture.CustomerRepository.GetAddressByIdAsync(address.Id);

            ////validacao
            var actionResult = Assert.IsType<Customer>(customerAdress.Result);
            var result = Assert.IsAssignableFrom<Customer>(actionResult);

            result.Name.Should().Be(customer.Name);
            result.Address.Should().NotBeNull();
        }

        public void Dispose()
        {
            _databaseFixture.Context.Database.ExecuteSqlCommand("delete from [dbo].Customer");
        }
    }
}
