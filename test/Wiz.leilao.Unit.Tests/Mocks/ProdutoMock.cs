using Bogus;
using Wiz.leilao.API.ViewModels.Customer;

namespace Wiz.leilao.Unit.Tests.Mocks
{
    public static class ProdutoMock
    {
        public static Faker<ProdutoViewModel> ProdutoViewModelFaker =>
            new Faker<ProdutoViewModel>()
            .CustomInstantiator(x => new ProdutoViewModel
            (
                id: x.Random.Number(1, 10),
                name: x.Person.FullName
            ));
    }
}
