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
        public DbSet<Product_Discount> Products_Discount { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
              .HasOne(o => o.User)          // Một đơn hàng (Order) thuộc về một User
              .WithMany(u => u.Orders)      // Một User có nhiều đơn hàng (Orders)
              .HasForeignKey(o => o.UserID) // Khóa ngoại UserID trong bảng Order
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
               .HasOne(o => o.Payment)         // Mỗi Order có một Payment
               .WithMany()                     // Một Payment có nhiều Order
               .HasForeignKey(o => o.PaymentID) // Khóa ngoại trong Order
               .OnDelete(DeleteBehavior.Restrict); // Hành động xử lý khi xóa Payment


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
