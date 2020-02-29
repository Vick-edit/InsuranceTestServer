using Microsoft.EntityFrameworkCore;
using ServiceCore.Domain.Models;

namespace ServiceCore.DataAccess.Mapping
{
    /// <summary>
    ///     Класс fluent - мапинга, вынесен отдельно, чтобы не плодить код в DbContext
    ///     так же за счёт таких классов будет проще перейти, например на тот же NHibernte
    ///     или Dapper полностью, т.к. они все поддерживают Fluent - мапинг и его регистрацию 
    /// </summary>
    public static class ProductMap
    {
        public static void Register(ModelBuilder modelBuilder)
        {
            // SQLITE не поддерживает последовательности
            /*
            modelBuilder.HasSequence<int>("ProductId_seq", schema: "Insurance")
                .StartsAt(1)
                .IncrementsBy(1)
                .HasMax(long.MaxValue)
                .IsCyclic(false);
            */

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Insurance");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    //.HasDefaultValueSql("NEXT VALUE FOR Insurance.ProductId_seq") // SQLITE не поддерживает последовательности
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.Description)
                    .HasColumnName("description");
            });
        }
    }
}