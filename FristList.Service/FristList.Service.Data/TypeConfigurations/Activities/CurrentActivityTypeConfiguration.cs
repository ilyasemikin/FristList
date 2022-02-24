using FristList.Service.Data.Models.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FristList.Service.Data.TypeConfigurations.Activities;

public class CurrentActivityTypeConfiguration : IEntityTypeConfiguration<CurrentActivity>
{
    public void Configure(EntityTypeBuilder<CurrentActivity> builder)
    {
        builder.HasKey(ca => ca.UserId);

        builder.HasOne(ca => ca.User)
            .WithOne()
            .IsRequired();

        builder.HasMany(ca => ca.Categories)
            .WithMany("CurrentActivities")
            .UsingEntity(type => type.ToTable("CurrentActivityCategories"));
    }
}