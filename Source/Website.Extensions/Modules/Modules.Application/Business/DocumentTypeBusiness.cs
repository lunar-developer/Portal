using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class DocumentTypeBusiness
    {
        public static List<DocumentTypeData> GetAllDocumentType()
        {
            return new DocumentTypeProvider().GetAllDocumentType();
        }

        public static DocumentTypeData GetDocumentType(string documentTypeID)
        {
            return new DocumentTypeProvider().GetDocumentType(documentTypeID);
        }

        public static string GetDisplayName(int documentTypeID)
        {
            return GetDisplayName(CacheBase.Receive<DocumentTypeData>(documentTypeID.ToString()))
                ?? documentTypeID.ToString();
        }

        public static string GetDisplayName(string documentCode)
        {
            return GetDisplayName(CacheBase.Find<DocumentTypeData>(DocumentTypeTable.DocumentCode, documentCode))
                ?? documentCode;
        }

        public static string GetDisplayName(DocumentTypeData documentType)
        {
            return documentType?.Name;
        }
    }
}