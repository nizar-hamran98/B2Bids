using Identity.Domain.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Models;
using SharedKernel;

namespace Identity.Domain.Mapping;

public static class UserMapping
{
    public static IEnumerable<UserModel> ToModels(this IEnumerable<User> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static UserModel? ToModel(this User entity)
    {
        return entity == null
            ? null
            : new UserModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                AccessFailedCount = entity.AccessFailedCount,
                Status = (EntityStatus)entity.StatusId,
                RoleId = entity.RoleId,
                IsNeverExpire = entity.IsNeverExpire,
                UserTypeCode = entity.UserTypeCode,
                Role = entity.Role != null && entity.Role?.StatusId == (short)EntityStatus.Active ? entity.Role.ToModel() : null,
                UserPermissions = entity.UserPermissions?.Where(up => up.StatusId == (short)EntityStatus.Active).ToModels().ToList(), 
                IsAllowAccess = entity.IsAllowAccess,
                RoleName = entity.Role?.Name
            };
    }
   
    public static User ToEntity(this UserModel model)
    {
        return new User
        {
            Id = model.Id,
            UserName = model.UserName.Trim(),
            NormalizedUserName = model.UserName.ToUpper().Trim(),
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            NormalizedEmail = model.Email.ToUpper().Trim(), 
            PhoneNumber = model.PhoneNumber.Trim(), 
            AccessFailedCount = model.AccessFailedCount,
            PasswordHash = model.Password,
            StatusId = (short)model.Status,
            IsNeverExpire = model.IsNeverExpire,
            RoleId = model.RoleId,
            UserTypeCode = model.UserTypeCode,
            IsAllowAccess = model.IsAllowAccess
        };
    }
    public static UserDTO? ToDto(this User entity)
    {
        return entity == null
            ? null
            : new UserDTO
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                LockoutEnabled = entity.LockoutEnabled,
                LockoutEnd = entity.LockoutEnd,
                AccessFailedCount = entity.AccessFailedCount,
                UserTypeCode = entity.UserTypeCode,
                ExternalCode = entity.ExternalCode,
            };
    }
    public static User? ToUpdatedEntity(this User entity, UserUpdateModel model)
    {
        if (entity is null) return null;

        entity.UserName = string.IsNullOrEmpty(model.UserName) ? entity.UserName : model.UserName.Trim();
        entity.NormalizedUserName = string.IsNullOrEmpty(model.UserName) ? entity.UserName : model.UserName.ToUpper().Trim();
        entity.FullName = string.IsNullOrEmpty(model.FullName) ? entity.FullName : model.FullName.Trim();
        entity.Email = string.IsNullOrEmpty(model.Email) ? entity.Email : model.Email.Trim();
        entity.NormalizedEmail = string.IsNullOrEmpty(model.Email) ? entity.Email : model.Email.ToUpper().Trim();
        entity.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? entity.PhoneNumber : model.PhoneNumber.Trim();
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;
        entity.RoleId = model.RoleId > 0 ? model.RoleId : entity.RoleId;
        entity.UserTypeCode = string.IsNullOrEmpty(model.UserTypeCode) ? entity.UserTypeCode : model.UserTypeCode;
        entity.IsNeverExpire = model.IsNeverExpire is not null? model.IsNeverExpire : entity.IsNeverExpire;
        entity.IsAllowAccess = model.IsAllowAccess is not null? model.IsAllowAccess.Value : entity.IsAllowAccess;

        return entity;
    }
}
