using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Infrastructure;
public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        #region Data Seed 
        modelBuilder.Entity<Role>();//.SeedDataWithUniqueLongId(Role.GenerateRoleSeedData());
        modelBuilder.Entity<User>().SeedDataWithUniqueLongId(User.GenerateUserSeedData());
        modelBuilder.Entity<Permissions>().SeedDataWithUniqueLongId(Permissions.GeneratePermissionsSeedData());
        modelBuilder.Entity<RolePermissions>();//.SeedDataWithUniqueLongId(RolePermissions.GenerateRolePermissionsSeedData());
        #endregion
    }
}