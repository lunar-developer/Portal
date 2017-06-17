namespace Modules.EmployeeManagement.Enum
{
    public static class MessageDefinitionEnum
    {
        public const string ColumnAtLineEmpty = "{0} tại dòng {1} không được RỖNG. Vui lòng cập nhật lại.";

        public const string ColumnAtLineIsNotDateFormat =
            "{0} tại dòng {1} không đúng định dạng ngày tháng (dd/mm/yyyy). Vui lòng cập nhật lại.";

        public const string FileExceedSize = "File vượt quá kích thước cho phép (5MB).";

        public const string FileImportFail =
            "Bạn đã upload danh sách cán bộ nhân viên KHÔNG thành công. Vui lòng kiểm tra lại.";

        public const string FileImportSuccess = "Bạn đã upload danh sách cán bộ nhân viên thành công.";
        public const string FileNotFollowTemplate = "File không đúng định dạng. Vui lòng sử dụng file theo mẫu.";
        public const string NoRecordFound = "(Không có kết quả nào)";
    }
}