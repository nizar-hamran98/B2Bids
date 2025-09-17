using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.MappingConfigurations;

public class UserPermissionsConfiguration : IEntityTypeConfiguration<UserPermissions>
{
    public void Configure(EntityTypeBuilder<UserPermissions> builder)
    {
         builder.ToTable("UserPermissions");
         builder.Property(x => x.Id).ValueGeneratedOnAdd();

    }
}
