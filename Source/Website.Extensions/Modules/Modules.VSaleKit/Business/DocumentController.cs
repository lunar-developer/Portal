using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modules.VSaleKit.Business
{
    public sealed class DocumentController 
    {
        private readonly DocumentProvider iProvider = new DocumentProvider();

        public List<DocumentInfo> GetListDocumentByPolicyId(string policyId)
        {
            try
            {
                return iProvider.GetListDocumentByPolicyId(policyId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        //public List<LegalInfo> GetListLegal()
        //{
        //    try
        //    {
        //        return iProvider.GetListLegal();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Dispose()
        {
            if(iProvider != null)
            {
                iProvider.Dispose();
            }
        }
       

        
    }
}
