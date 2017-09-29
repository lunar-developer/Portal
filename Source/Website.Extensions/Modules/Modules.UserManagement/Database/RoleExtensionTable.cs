namespace Modules.UserManagement.Database
{
    public static class RoleExtensionTable
    {
        public const string RoleGroupID = "RoleGroupID";
        public const string RoleScope = "RoleScope";
        public const string ListExcludeRoleID = "ListExcludeRoleID";

        public const string StoreGetRoleExtension = "dbo.UM_SP_GetRoleExtension";
        public const string StoreUpdateRoleExtension = "dbo.UM_SP_UpdateRoleExtension";
        public const string StoreDeleteRoleExtension = "dbo.UM_SP_DeleteRoleExtension";
    }
}