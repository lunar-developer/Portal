using Telerik.Web.UI;
using Telerik.Web.UI.AsyncUpload;

namespace Modules.Controls
{
    public sealed class FileUpload : RadAsyncUpload
    {
        public FileUpload()
        {
            RenderMode = RenderMode.Lightweight;
        }
    }
}