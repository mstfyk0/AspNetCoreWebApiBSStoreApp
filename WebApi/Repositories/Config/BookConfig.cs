using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasData(
                    new Book { Id = 1, Title = "Hacivat ile karagöz", Price = 100 },
                    new Book { Id = 2, Title = "Sherlock Holmes", Price = 67 },
                    new Book { Id = 3, Title = "Kuantum fiziğini öğreniyoruz.", Price = 1689 }


                );
        }
    }
}
