using Microsoft.EntityFrameworkCore;
using PcWarehouseApi.Models;

namespace PcWarehouseApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PC>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Weight)
                .IsRequired();

            entity.Property(e => e.Warranty)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.Stock)
                .IsRequired();
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Abbreviation)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Abbreviation)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.FoundationDate)
                .IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code)
                .HasMaxLength(10);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.Description)
                .IsRequired();

            entity.HasOne(e => e.ComponentManufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(e => e.ComponentManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ComponentType)
                .WithMany(t => t.Components)
                .HasForeignKey(e => e.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });

            entity.Property(e => e.Amount)
                .IsRequired();

            entity.HasOne(e => e.PC)
                .WithMany(pc => pc.PCComponents)
                .HasForeignKey(e => e.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(c => c.PCComponents)
                .HasForeignKey(e => e.ComponentCode)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}