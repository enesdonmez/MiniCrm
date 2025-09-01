using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Data;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<CustomerNote> CustomerNotes => Set<CustomerNote>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
        
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Id).IsUnique();
            entity.Property(c => c.FullName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Company).IsRequired().HasMaxLength(250);
            entity.Property(c => c.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Id).IsUnique();
            entity.Property(c => c.Id).IsRequired();
            entity.Property(c => c.CustomerId).IsRequired();
            entity.Property(c => c.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.Title).IsRequired().HasMaxLength(250);
            entity.Property(c => c.Status).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<CustomerNote>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Id).IsUnique();
            entity.Property(c => c.Id).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.Note).IsRequired().HasMaxLength(500);
            entity.Property(c => c.CustomerId).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u  => u.Id);
            entity.HasIndex(u => u.Id).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
        });
        
        modelBuilder.Entity<CustomerNote>()
            .HasOne(cn => cn.Customer)
            .WithMany() 
            .HasForeignKey(cn => cn.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Deal>()
            .HasOne(cn => cn.Customer)
            .WithMany() 
            .HasForeignKey(cn => cn.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }


}