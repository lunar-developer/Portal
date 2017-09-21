using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class DocumentTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : DocumentTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (DocumentTypeData item in DocumentTypeBusiness.GetAllDocumentType())
            {
                dictionary.TryAdd(item.DocumentTypeID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string documentTypeID)
        {
            return DocumentTypeBusiness.GetDocumentType(documentTypeID);
        }
    }
}