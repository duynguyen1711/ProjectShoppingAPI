using Microsoft.EntityFrameworkCore;
using TrainingBE.Model;
namespace TrainingBE.Data
{
    public class MyDBContext: DbContext 
    {
        public MyDBContext(DbContextOptions<MyDBContext> options):base(options) { 
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product_Discount> Products_Discount { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasMany(p => p.Discount)
            .WithOne()
            .IsRequired(false);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryID);
        }
    }
}
