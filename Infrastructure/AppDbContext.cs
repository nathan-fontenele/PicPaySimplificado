using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;

namespace PicPaySimplificado.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<CommonUsersEntity> Users { get; set; }
        public DbSet<SellerUserEntity> Seller { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonUsersEntity>()
                    .HasKey(u => u.Guid); 

            modelBuilder.Entity<CommonUsersEntity>().HasIndex(u => u.Cpf)
                .IsUnique();

            modelBuilder.Entity<CommonUsersEntity>().HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<SellerUserEntity>()
                .HasKey(u => u.Guid); 

            modelBuilder.Entity<SellerUserEntity>().HasIndex(u => u.Cnpj)
                .IsUnique();

            modelBuilder.Entity<SellerUserEntity>().HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
