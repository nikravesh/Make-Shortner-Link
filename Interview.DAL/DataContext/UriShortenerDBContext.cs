using Interview.Model.UriShortener.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interview.DAL.DataContext;

public class UriShortenerDBContext : DbContext
{
    public DbSet<UriShortenerEntity> UriShorteners { get; set; }

    public UriShortenerDBContext(DbContextOptions<UriShortenerDBContext> options)
        :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
