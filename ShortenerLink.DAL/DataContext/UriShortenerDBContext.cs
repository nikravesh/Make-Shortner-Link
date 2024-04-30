using Microsoft.EntityFrameworkCore;

using ShortenerLink.Model.UriShortener.Entities;

namespace ShortenerLink.DAL.DataContext;

public class UriShortenerDBContext : DbContext
{
    public DbSet<UriShortenerEntity> UriShorteners { get; set; }

    public UriShortenerDBContext(DbContextOptions<UriShortenerDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
