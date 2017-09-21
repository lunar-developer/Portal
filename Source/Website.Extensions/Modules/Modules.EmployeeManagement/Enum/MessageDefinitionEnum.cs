namespace Modules.EmployeeManagement.Enum
{
    public static class MessageDefinitionEnum
    {
        public const string ColumnAtLineEmpty = "{0} tại dòng {1} không được RỖNG. Vui lòng cập nhật lại.";

        public const string ColumnAtLineIsNotDateFormat =
            "{0} tại dòng {1} không đúng định dạng ngày tháng (yyyyMMdd). Vui lòng cập nhật lại.";

        public const string FileExceedSize = "File vượt quá kích thước cho phép (5MB).";

        public const string FileImportFail =
            "Bạn đã upload danh sách cán bộ nhân viên KHÔNG thành công. Vui lòng kiểm tra lại.";

        public const string FileImportSuccess = "Bạn đã upload danh sách cán bộ nhân viên thành công.";
        public const string FileNotFollowTemplate = "File không đúng định dạng. Vui lòng sử dụng file theo mẫu.";
        public const string NoRecordFound = "(Không có kết quả nào)";
        public const string NoDataFound = "Không tìm thấy thông tin liên lạc của bạn";
        public const string FileNotFound = "Vui lòng chọn file ảnh.";
        public const string FileNotImage = "Không phải định dạng file ảnh.";
        public const string UpdateEmployeeImageSuccess = "Cập nhật hình ảnh nhân viên thành công.";
        public const string UpdateEmployeeImageFail = "Cập nhật hình ảnh nhân viên KHÔNG thành công.";
        public const string UpdateEmployeeSuccess = "Cập nhật thông tin cá nhân THÀNH CÔNG.";
        public const string UpdateEmployeeFail = "Cập nhật thông tin cá nhân KHÔNG THÀNH CÔNG.";
    }
}