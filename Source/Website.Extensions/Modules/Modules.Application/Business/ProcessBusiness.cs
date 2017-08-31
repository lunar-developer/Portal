using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class ProcessBusiness
    {
        public static List<ProcessData> GetAllProcessData()
        {
            return new ProcessProvider().GetAllProcessData();
        }

        public static ProcessData GetProcesseData(string phaseID)
        {
            return new ProcessProvider().GetProcesseData(phaseID);
        }

        public static string GetProcessName(string processID)
        {
            ProcessData processData = CacheBase.Receive<ProcessData>(processID);
            return processData?.Name ?? processID;
        }
    }
}