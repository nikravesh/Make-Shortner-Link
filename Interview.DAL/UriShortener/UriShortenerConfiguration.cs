using Interview.Model.UriShortener.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview.DAL.UriShortener;

public class UriShortenerConfiguration : IEntityTypeConfiguration<UriShortenerEntity>
{
    public void Configure(EntityTypeBuilder<UriShortenerEntity> builder)
    {
        builder.Property(u => u.OrginalUri).HasMaxLength(1000);
        builder.Property(u => u.ShortenerUri).HasMaxLength(100);
    }
}
