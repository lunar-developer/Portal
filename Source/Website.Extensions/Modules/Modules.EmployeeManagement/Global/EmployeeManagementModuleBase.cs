using Website.Library.Global;

namespace Modules.EmployeeManagement.Global
{
    public class EmployeeManagementModuleBase : DesktopModuleBase
    {
        private static readonly string TemplateUrl =
            FunctionBase.GetAbsoluteUrl(
                "/DesktopModules/Modules/EmployeeManagement/Assets/Template/Template_Employee.xlsx");

        protected string LinkTemplate => TemplateUrl;
    }
}