using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Customer> Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
