using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class DocumentTypeCacheBusiness<T> : ICache where T : DocumentTypeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (DocumentTypeData item in DocumentTypeBusiness.GetAllDocumentType())
            {
                dictionary.TryAdd(item.DocumentTypeID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string documentTypeID)
        {
            return DocumentTypeBusiness.GetDocumentType(documentTypeID);
        }
    }
}