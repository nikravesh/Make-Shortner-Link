using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ShortenerLink.Model.UriShortener.Entities;

namespace ShortenerLink.DAL.UriShortener;

public class UriShortenerConfiguration : IEntityTypeConfiguration<UriShortenerEntity>
{
    public void Configure(EntityTypeBuilder<UriShortenerEntity> builder)
    {
        builder.Property(u => u.OrginalUri).HasMaxLength(1000);
        builder.Property(u => u.ShortenerUri).HasMaxLength(100);
    }
}
