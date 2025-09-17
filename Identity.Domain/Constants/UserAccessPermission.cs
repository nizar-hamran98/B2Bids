namespace Identity.Domain.Constants;
public static class UserAccessPermission
{
    public const string ReadUsers = "ReadUsers";
    public const string CreateUsers = "CreateUsers";
    public const string ModifyUsers = "ModifyUsers";
    public const string RemoveUsers = "RemoveUsers";
    public const string UpdateUserTimeZone = "UpdateUserTimeZone";
    public const string ReadTimeZones = "ReadTimeZones";

    public const string ReadRoles = "ReadRoles";
    public const string CreateRoles = "CreateRoles";
    public const string ModifyRoles = "ModifyRoles";
    public const string RemoveRoles = "RemoveRoles";

    public const string ReadPermissions = "ReadPermissions";
    public const string CreatePermissions = "CreatePermissions";
    public const string ModifyPermissions = "ModifyPermissions";
    public const string RemovePermissions = "RemovePermissions";

    public const string ReadUserPermissions = "ReadUserPermissions";
    public const string CreateUserPermissions = "CreateUserPermissions";
    public const string ModifyUserPermissions = "ModifyUserPermissions";
    public const string RemoveUserPermissions = "RemoveUserPermissions";

    public static IEnumerable<string> GetIdentityPermissionNames()
    {

        yield return ReadUsers;
        yield return CreateUsers;
        yield return ModifyUsers;
        yield return RemoveUsers;
        yield return UpdateUserTimeZone;
        yield return ReadTimeZones;

        yield return ReadRoles;
        yield return CreateRoles;
        yield return ModifyRoles;
        yield return RemoveRoles;

        yield return ReadPermissions;
        yield return CreatePermissions;
        yield return ModifyPermissions;
        yield return RemovePermissions;

        yield return ReadUserPermissions;
        yield return CreateUserPermissions;
        yield return ModifyUserPermissions;
        yield return RemoveUserPermissions;
    }
}
