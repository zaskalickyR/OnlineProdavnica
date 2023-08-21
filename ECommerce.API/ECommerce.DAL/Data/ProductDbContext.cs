
using ECommerce.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ECommerce.DAL.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) 
        { 

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            //builder.Entity<OrderItem>()
            //    .HasKey(cp => new { cp.OrderId, cp.ProductId });

            //builder.Entity<OrderItem>()
            //    .HasOne(cp => cp.Order)
            //    .WithMany(p => p.OrderItems)
            //    .HasForeignKey(cp => cp.OrderId);
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
