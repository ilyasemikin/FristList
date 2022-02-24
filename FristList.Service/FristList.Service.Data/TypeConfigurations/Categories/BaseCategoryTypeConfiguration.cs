using FristList.Service.Data.Models.Categories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FristList.Service.Data.TypeConfigurations.Categories;

public class BaseCategoryTypeConfiguration : IEntityTypeConfiguration<BaseCategory>
{
    public void Configure(EntityTypeBuilder<BaseCategory> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired();
    }
}