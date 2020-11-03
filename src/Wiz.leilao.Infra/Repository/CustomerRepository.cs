using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiz.leilao.Domain.Interfaces.Repository;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Domain.Models.Dapper;
using Wiz.leilao.Infra.Context;

namespace Wiz.leilao.Infra.Repository
{
    public class CustomerRepository : EntityBaseRepository<Customer>, ICustomerRepository
    {
        private readonly DapperContext _dapperContext;

        public CustomerRepository(EntityContext context, DapperContext dapperContext)
            : base(context)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<CustomerAddress>> GetAllAsync()
        {
            var query = @"SELECT c.Id, a.Id AS AddressId, c.Name, c.DateCreated, a.CEP
                            FROM dbo.Customer c
                            INNER JOIN dbo.Address a
                            ON c.AddressId = a.Id";

            return await _dapperContext.DapperConnection.QueryAsync<CustomerAddress>(query);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var query = @"SELECT Id, AddressId, Name, DateCreated
                          FROM dbo.Customer c
                          WHERE c.Id = @Id";

            return await _dapperContext.DapperConnection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<Customer> GetAddressByIdAsync(int id)
        {
            var query = @"SELECT *
                          FROM dbo.Customer c
                          INNER JOIN dbo.Address a
                          ON c.AddressId = a.Id
                          WHERE c.Id = @Id";

            var customer = _dapperContext.DapperConnection.Query<Customer, Address, Customer>(
                query,
                map: (customer, address) =>
                {
                    customer.Address = address;
                    return customer;
                },
                new { Id = id });

            return await Task.FromResult(customer.FirstOrDefault());
        }

        public async Task<CustomerAddress> GetByNameAsync(string name)
        {
            var query = @"SELECT c.Id, a.Id AS AddressId, c.Name, c.DateCreated, a.CEP
                          FROM dbo.Customer c
                          INNER JOIN dbo.Address a
                          ON c.AddressId = a.Id
                          WHERE c.Name = @Name";

            return await _dapperContext.DapperConnection.QueryFirstOrDefaultAsync<CustomerAddress>(query, new { Name = name });
        }
    }
}
