﻿using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ApplicationTypeData : CacheData
    {
        public string ApplicationTypeID { get; set; }
        public string ApplicationTypeCode { get; set; }
        public string Name { get; set; }
        public string SortOrder { get; set; }
        public string IsDisable { get; set; }
    }
}