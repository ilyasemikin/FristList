using FristList.Service.Data.Models.Categories;
using FristList.Service.Data.Models.Categories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FristList.Service.Data.TypeConfigurations.Categories;

public class PersonalCategoryTypeConfiguration : IEntityTypeConfiguration<PersonalCategory>
{
    public void Configure(EntityTypeBuilder<PersonalCategory> builder)
    {
        builder.HasBaseType<BaseCategory>();

        builder.HasOne(c => c.Owner)
            .WithMany()
            .IsRequired();
    }
}