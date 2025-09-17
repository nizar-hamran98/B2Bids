using SharedKernel;

namespace Identity.Domain.Entities;
public class Permissions : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }

    public static Permissions[] GeneratePermissionsSeedData()
    {
        return new Permissions[]
        {
        new()
        {
            Id = 1,
            Code = "ReadUsers",
            Name = "Read Users",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 2,
            Code = "CreateUsers",
            Name = "Create Users",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 3,
            Code = "ModifyUsers",
            Name = "Modify Users",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 4,
            Code = "RemoveUsers",
            Name = "Remove Users",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 5,
            Code = "ReadRoles",
            Name = "Read Roles",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 6,
            Code = "CreateRoles",
            Name = "Create Roles",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id =7,
            Code = "ModifyRoles",
            Name = "Modify Roles",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 8,
            Code = "RemoveRoles",
            Name = "Remove Roles",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 9,
            Code = "ReadPermissions",
            Name = "Read Permissions",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 10,
            Code = "CreatePermissions",
            Name = "Create Permissions",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 11,
            Code = "ModifyPermissions",
            Name = "Modify Permissions",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 12,
            Code = "RemovePermissions",
            Name = "Remove Permissions",
            CreatedAt = new DateTime(2023, 4, 1, 6, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 13,
            Code = "CreateUserPermissions",
            Name = "CreateUserPermissions",
            CreatedAt = new DateTime(2023, 5, 3, 3, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 14,
            Code = "RemoveUserPermissions",
            Name = "RemoveUserPermissions",
            CreatedAt = new DateTime(2023, 5, 3, 3, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 15,
            Code = "UpdateUserTimeZone",
            Name = "UpdateUserTimeZone",
            CreatedAt = new DateTime(2024, 3, 21, 16, 59, 23),
            StatusId = 1
        },
        new()
        {
            Id = 16,
            Code = "ReadTimeZones",
            Name = "ReadTimeZones",
            CreatedAt = new DateTime(2024, 3, 21, 16, 59, 23),
            StatusId = 1
        },
        new()
        {
            Id = 17,
            Code = "ModifyUserPermissions",
            Name = "ModifyUserPermissions",
            CreatedAt = new DateTime(2023, 5, 3, 3, 0, 0),
            StatusId = 1
        },
        new()
        {
            Id = 18,
            Code = "ReadUserPermissions",
            Name = "ReadUserPermissions",
            CreatedAt = new DateTime(2023, 5, 3, 3, 0, 0),
            StatusId = 1
        }
    };
    }
}

