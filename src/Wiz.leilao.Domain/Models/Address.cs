using System.Collections.Generic;

namespace Wiz.leilao.Domain.Models
{
    public class Address
    {
        protected Address()
        {
            Customers = new HashSet<Customer>();
        }

        public Address(string cep)
        {
            CEP = cep;
        }

        public int Id { get; set; }
        public string CEP { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
