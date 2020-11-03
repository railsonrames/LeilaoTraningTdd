using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Wiz.leilao.Domain.Interfaces.Repository;
using Wiz.leilao.Domain.Models;

namespace Wiz.leilao.Domain.Validation.CustomerValidation
{
    public class CustomerInsertValidation : AbstractValidator<Customer>, ICustomerInsertValidation
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerInsertValidation(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.AddressId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Endereço não pode ser nulo");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Nome não pode ser nulo");

            RuleFor(x => x)
                .MustAsync(ValidationName)
                .WithMessage("Nome já cadastrado na base de dados");
        }

        private async Task<bool> ValidationName(Customer customer, CancellationToken cancellationToken)
        {
            var customerRepository = await _customerRepository.GetByNameAsync(customer.Name);

            return customer.Name != customerRepository?.Name;
        }
    }
}
