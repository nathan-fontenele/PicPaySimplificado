using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey("_guid");
                
                entity.Property<Guid>("_guid")
                    .HasColumnName("Guid")
                    .IsRequired();

                entity.Property<string>("_fullname")
                    .HasColumnName("Fullname")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property<string>("_email")
                    .HasColumnName("Email")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.HasIndex("_email").IsUnique();

                entity.Property<string>("_password")
                    .HasColumnName("Password")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property<decimal>("_balance")
                    .HasColumnName("Balance")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property<UserType>("_userType")
                    .HasColumnName("UserType")
                    .HasConversion<string>()
                    .IsRequired();

                entity.OwnsOne(typeof(Document), "_document", navBuilder =>
                {
                    navBuilder.Property<string>("DocumentNumber")
                        .HasColumnName("Document")
                        .IsRequired();

                    navBuilder.HasIndex("DocumentNumber").IsUnique();
                });
            });


            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(t => t.Created)
                    .HasColumnName("Created")
                    .HasColumnType("datetime");
                
                //Aqui fica o relacionamento 1:N
                //Um usuário pode ter diversas transações
                entity.HasOne(t => t.Sender)
                    .WithMany(u => u.SentTransactions)
                    .HasForeignKey(t => t.SenderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Receiver)
                    .WithMany(u => u.ReceivedTransactions)
                    .HasForeignKey(t => t.ReceiverId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
