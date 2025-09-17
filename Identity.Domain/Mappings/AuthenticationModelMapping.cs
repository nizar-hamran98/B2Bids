using Identity.Domain.Entities;
using Identity.Domain.Models;

namespace Identity.Domain.Mapping;
public static class AuthenticationModelMapping
{
    public static IEnumerable<AuthenticationModel> ToModels(this IEnumerable<AuthenticationEntity> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static AuthenticationModel? ToModel(this AuthenticationEntity entity)
    {
        return entity == null
            ? null
            : new AuthenticationModel
            {
                UserName = entity.UserName,
                Email = entity.Email,
                Password = entity.Password,
            };
    }

    public static AuthenticationEntity ToEntity(this AuthenticationModel model)
    {
        return new AuthenticationEntity
        {
            UserName = model.UserName,
            Email = model.Email,
            Password = model.Password,
        };
    }
}
