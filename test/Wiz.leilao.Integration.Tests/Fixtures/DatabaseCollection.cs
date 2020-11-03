using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Wiz.leilao.Integration.Tests.Fixtures
{
    [CollectionDefinition("DatabaseTestCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
