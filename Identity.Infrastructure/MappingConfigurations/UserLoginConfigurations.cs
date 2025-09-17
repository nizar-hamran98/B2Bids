using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.MappingConfigurations;

internal class UserLoginConfigurations : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
         builder.ToTable("UserLogin");
         builder.HasKey(x => x.Id);
         builder.HasIndex(x => x.UserId).IsUnique();

    }
}
