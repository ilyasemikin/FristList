using FristList.Service.Data.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FristList.Service.Data.TypeConfigurations.Account;

public class RefreshTokenTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(t => t.Token)
            .IsRequired()
            .IsUnicode(false);
        builder.HasIndex(t => t.Token)
            .IsUnique();
        builder.Property(t => t.ExpiresAt)
            .IsRequired();
        builder.HasOne(t => t.User)
            .WithMany()
            .IsRequired();
    }
}