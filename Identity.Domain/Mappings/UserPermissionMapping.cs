using Identity.Domain.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Models;
using SharedKernel;

namespace Identity.Domain.Mapping;

public static class UserPermissionMapping
{
    public static IEnumerable<UserPermissionModel> ToModels(this IEnumerable<UserPermissions> entities) => entities.Select(x => x.ToModel());
    public static IEnumerable<UserPermissionDTO> ToDtos(this IEnumerable<UserPermissions> entities) => entities.Select(x => x.ToDto());

    public static UserPermissionModel? ToModel(this UserPermissions entity)
    {
        return entity == null
            ? null
            : new UserPermissionModel
            {
                PermissionId = entity.PermissionId,
                UserId = entity.UserId,
                Id = entity.Id,
                Status = (EntityStatus)entity.StatusId,
                PermissionCode=entity.Permission.Code      

            };
    }

    public static UserPermissions? ToEntity(this UserPermissionModel model)
    {
        return model == null
            ? null
            : new UserPermissions
            {
                PermissionId = model.PermissionId,
                UserId = model.UserId,
                Id = model.Id,
                StatusId = (short)model.Status,
            };
    }
    public static UserPermissionDTO? ToDto(this UserPermissions entity)
    {
        if (entity == null) return null;

        return new UserPermissionDTO
        {
            PermissionId = entity.PermissionId,
            PermissionCode = entity.Permission?.Code,  
            UserId = entity.UserId,
            Id = entity.Id,
            Status = (EntityStatus)entity.StatusId,
        };
    }



}
