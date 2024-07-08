using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Enities;

namespace Orders.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){ }
    public DbSet<PRODUCTS> PRODUCTS { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<PRODUCTS>(entity =>
        {
            entity.ToTable("PRODUCTS");
            entity.HasKey(e => e.ORDERID);
            // Cấu hình các thuộc tính khác của PRODUCTS nếu cần
        });
    }
}