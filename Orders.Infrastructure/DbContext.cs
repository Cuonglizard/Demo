﻿using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Enities;

namespace Orders.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){ }
    public DbSet<PRODUCTS> PRODUCTS { get; set; }
    public DbSet<ITEM> ITEM { get; set; }
    public DbSet<PAYMENT> PAYMENT { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<PRODUCTS>(entity =>
        {
            entity.ToTable("PRODUCTS");
            entity.HasKey(e => e.ORDERID);
            // Cấu hình các thuộc tính khác của PRODUCTS nếu cần
        });
        modelBuilder.Entity<ITEM>(entity =>
        {
            entity.ToTable("ITEM");
            entity.HasKey(e => e.ID);
            // Cấu hình các thuộc tính khác của PRODUCTS nếu cần
        });
        modelBuilder.Entity<PAYMENT>(entity =>
        {
            entity.ToTable("PAYMENT");
            entity.HasKey(e => e.ID);
        });
    }
}