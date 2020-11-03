using System;

namespace Wiz.leilao.API.ViewModels.Customer
{
    public class ProdutoViewModel
    {
        public ProdutoViewModel() { }

        public ProdutoViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
