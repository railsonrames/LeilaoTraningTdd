using System.Collections.Generic;
using System.Threading.Tasks;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Domain.Models.Dapper;

namespace Wiz.leilao.Domain.Interfaces.Repository
{
    public interface ICustomerRepository : IEntityBaseRepository<Customer>, IDapperReadRepository<Customer>
    {
        Task<IEnumerable<CustomerAddress>> GetAllAsync();

        Task<Customer> GetAddressByIdAsync(int id);
        
        Task<CustomerAddress> GetByNameAsync(string name);
    }
}
