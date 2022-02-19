using FristList.Service.Data.Models.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FristList.Service.Data.TypeConfigurations;

public class ActivityCategoryTypeConfiguration : IEntityTypeConfiguration<ActivityCategory>
{
    public void Configure(EntityTypeBuilder<ActivityCategory> builder)
    {
        builder.HasOne(ac => ac.Activity)
            .WithMany(a => a.Categories)
            .HasForeignKey(ac => ac.ActivityId);

        builder.HasOne(ac => ac.Category)
            .WithMany()
            .HasForeignKey(ac => ac.CategoryId);

        builder.HasKey(ac => new { ac.ActivityId, ac.CategoryId });
    }
}