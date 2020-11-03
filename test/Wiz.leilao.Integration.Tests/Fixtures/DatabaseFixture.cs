using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Wiz.leilao.Domain.Interfaces.Identity;
using Wiz.leilao.Infra.Context;
using Wiz.leilao.Infra.Repository;

namespace Wiz.leilao.Integration.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public DapperContext _dapper;
        public IConfiguration _configuration;
        
        public DatabaseFixture()
        {
            var connectionString = $"Server=localhost;Database=leilao_test;User ID=sa;Password=Q1w2e3r4!;Trusted_Connection=False;";
            ContextOptions = new DbContextOptionsBuilder<EntityContext>()
                                .UseSqlServer(connectionString)
                                .EnableSensitiveDataLogging()
                                .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddDebug(); }))
                                .Options;

            var myConfiguration = new Dictionary<string, string>
            {
                { "ConnectionStrings:LeilaoDB", connectionString }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            IdentityServiceMock = new Mock<IIdentityService>();
         
            //IdentityServiceMock.Setup(x => x.GetTenantId())
            //    .Returns("Wx1");

            Context = new EntityContext(ContextOptions);
            _dapper = new DapperContext(_configuration);

            Seed();
        }

        public CustomerRepository CustomerRepository { get { return new CustomerRepository(Context, _dapper); } }
        
        protected DbContextOptions<EntityContext> ContextOptions 
        { 
            get; 
        }

        public Mock<IIdentityService> IdentityServiceMock { 
            get; 
            set; 
        }
        
        public EntityContext Context 
        { 
            get; 
            set; 
        }

        protected virtual void Seed()
        {
            using (var context = new EntityContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        protected DbContext GetFixtureContext()
        {
            return new EntityContext(ContextOptions);
        }

        public void Dispose()
        {
        }
    }
}