using System.Threading.Tasks;

namespace Wiz.leilao.Domain.Interfaces.Repository
{
    public interface IDapperReadRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
    }
}
