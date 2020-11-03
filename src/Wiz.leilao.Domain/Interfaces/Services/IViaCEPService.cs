using System.Threading.Tasks;
using Wiz.leilao.Domain.Models.Services;

namespace Wiz.leilao.Domain.Interfaces.Services
{
    public interface IViaCEPService
    {
        Task<ViaCEP> GetByCEPAsync(string cep);
    }
}
