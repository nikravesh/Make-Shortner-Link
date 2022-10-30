using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview.DAL.UriShortener;

public class UriShortenerConfiguration : IEntityTypeConfiguration<Model.UriShortener.Entities.UriShortener>
{
    public void Configure(EntityTypeBuilder<Model.UriShortener.Entities.UriShortener> builder)
    {
        builder.Property(u => u.OrginalUri).HasMaxLength(1000);
        builder.Property(u => u.ShortenerUri).HasMaxLength(100);
    }
}
