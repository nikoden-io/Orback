using Application.Abstractions;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("app");
        b.Entity<Customer>(e =>
        {
            e.ToTable("customers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Email)
             .HasColumnName("email")
             .HasMaxLength(256)
             .IsRequired();
        });
    }
}
