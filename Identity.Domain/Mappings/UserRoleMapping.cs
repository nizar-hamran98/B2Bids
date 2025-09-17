using Identity.Domain.Entities;
using Identity.Domain.Models;

namespace Identity.Domain.Mapping;

public static class UserRoleMapping
{

    public static IEnumerable<UserRoleModel> ToModels(this IEnumerable<UserRole> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static UserRoleModel ToModel(this UserRole entity)
    {
        return entity == null
            ? null
            : new UserRoleModel
            {
                Id = entity.Id,
                RoleId = entity.RoleId,
                UserId = entity.UserId,
                Role = entity.Role.ToModel()
            };
    }
}
