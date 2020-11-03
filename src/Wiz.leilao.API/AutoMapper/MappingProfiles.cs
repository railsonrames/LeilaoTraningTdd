using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Wiz.leilao.API.ViewModels.Customer;
using Wiz.leilao.Domain.Models;
using Wiz.leilao.Domain.Models.Dapper;

namespace Wiz.leilao.API.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Customer

            CreateMap<CustomerAddress, CustomerAddressViewModel>();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();

            #endregion
        }
    }
}
