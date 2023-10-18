using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess
{
    internal class CatalogDbContext : DbContext
    {

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Item> Items { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity => {

                entity.ToTable("category");

                entity.Property(category => category.Id)
                .HasColumnName("category_id");

                entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");

                entity.Property(category => category.Url)
                .HasColumnName("url");

                entity.HasOne(category => category.ParentCategory)
                .WithMany()
                .HasForeignKey(category => category.ParentCategoryId);
            });

            modelBuilder.Entity<Item>(entity => {

                entity.ToTable("item");

                entity.Property(item => item.Id)
                .HasColumnName("item_id");

                entity.Property(item => item.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");

                entity.Property(item => item.Description)
                .HasColumnName("description");

                entity.Property(item => item.Url)
                .HasColumnName("url");

                entity.HasOne(item => item.Category)
                .WithMany()
                .HasForeignKey(item => item.CategoryId);

               entity.Property(item => item.Price)
               .IsRequired()
               .HasColumnName("price");

               entity.Property(item => item.Amount)
               .IsRequired()
               .HasColumnName("amount");

            });
        }

    }
}
