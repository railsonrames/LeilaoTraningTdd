using System.Collections.Generic;
using System.Threading.Tasks;
using Wiz.leilao.API.ViewModels.Customer;

namespace Wiz.leilao.API.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoViewModel>> GetAllAsync();
    }
}
