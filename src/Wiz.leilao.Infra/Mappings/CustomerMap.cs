using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wiz.leilao.Domain.Models;

namespace Wiz.leilao.Infra.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("VARCHAR(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.AddressId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(x => x.DateCreated)
                .HasColumnType("DATETIME2")
                .IsRequired();

            builder.HasOne(x => x.Address)
                .WithMany(x => x.Customers)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
