namespace Modules.Disbursement.Enum
{
    internal static class RoleEnum
    {
        public const string Input = "DB_Input";
        public const string Revise = "DB_ApproveAtBranch";
        //public const string Preapprove = "DB_ApproveAtBranch";
        public const string Approve = "DB_ApproveAtSME";
        public const string Alm = "DB_AdjustRoomALM";
    }
}