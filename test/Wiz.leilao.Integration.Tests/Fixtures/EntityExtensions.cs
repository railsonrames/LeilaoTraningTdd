using Microsoft.Extensions.Configuration;
using Wiz.leilao.Infra.Context;

namespace Wiz.leilao.Integration.Tests.Fixtures
{
    public static class EntityExtensions
    {
        public static T Persist<T>(this T entity, EntityContext context) where T : class
        {
            context.Add(entity);
            context.SaveChanges();
            
            return entity;
        }
    }
}