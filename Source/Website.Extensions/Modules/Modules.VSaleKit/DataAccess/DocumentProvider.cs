using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Website.Library.Enum;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Enum;

namespace Modules.VSaleKit.DataAccess
{
    public class DocumentProvider : DatabaseCtrl// DataProvider
    {
        public DocumentProvider() : base(ConnectionEnum.VSaleKit)
        {
            ;
        }
        public List<DocumentInfo> GetListDocumentByPolicyId(string policyId)
        {
            List<DocumentInfo> docLst = new List<DocumentInfo>();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                //Connector.AddParameter("PolicyID", SqlDbType.VarChar, policyId);
                //Connector.ExecuteProcedure("GetListDocument", out ds);
                SqlCommand cmd = new SqlCommand("[GetListDocument]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@PolicyID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, policyId));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "DOCLIST");

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables["DOCLIST"].Rows)
                    {
                        DocumentInfo doc = new DocumentInfo();
                        doc.DocId = r["DocID"] == DBNull.Value ? "" : r["DocID"].ToString();
                        doc.DocName = r["DocName"] == DBNull.Value ? "" : r["DocName"].ToString();
                        docLst.Add(doc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return docLst;
        }

        //public List<LegalInfo> GetListLegal()
        //{
        //    List<LegalInfo> legalLst = new List<LegalInfo>();
        //    string lastDate = "";
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da;
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("[GetListLegal]", dbVS.Connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandTimeout = AppSettings.DatabaseTimeout;

        //        //cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@SystemType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Constants.LEGAL_TYPE));
        //        if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
        //        {
        //            dbVS.Connection.Open();
        //        }
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(ds, "LEGALLIST");
        //        if (ds.Tables.Count > 0)
        //        {
        //            foreach (DataRow r in ds.Tables["LEGALLIST"].Rows)
        //            {
        //                LegalInfo legal = new LegalInfo();
        //                legal.LegalId = r["IdentityTypeCode"] == DBNull.Value ? "" : r["IdentityTypeCode"].ToString();
        //                legal.LegalName = r["Name"] == DBNull.Value ? "" : r["Name"].ToString();
        //                string date = r["ModifyDateTime"] == DBNull.Value ? "0" : r["ModifyDateTime"].ToString();
        //                if (date.CompareTo(lastDate) > 0)
        //                {
        //                    lastDate = date;
        //                }
        //                legalLst.Add(legal);
        //            }
        //            foreach (LegalInfo legal in legalLst)
        //            {
        //                legal.LatestUpdate = lastDate;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return legalLst;
        //}

        //public List<LegalInfo> GetListLegal()
        //{
        //    List<LegalInfo> legalLst = new List<LegalInfo>();
        //    string lastDate = "";
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da;
        //    try
        //    {
        //        //Connector.AddParameter("SystemType", SqlDbType.VarChar, Constants.LEGAL_TYPE);
        //        //Connector.ExecuteProcedure("GetSysPara", out ds);
        //        SqlCommand cmd = new SqlCommand("GetSysPara", dbVS.Connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandTimeout = AppSettings.DatabaseTimeout;

        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@SystemType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Constants.LEGAL_TYPE));
        //        if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
        //        {
        //            dbVS.Connection.Open();
        //        }
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(ds, "LEGALLIST");
        //        if (ds.Tables.Count > 0)
        //        {
        //            foreach (DataRow r in ds.Tables["LEGALLIST"].Rows)
        //            {
        //                LegalInfo legal = new LegalInfo();
        //                legal.LegalId = r["TypeID"] == DBNull.Value ? "" : r["TypeID"].ToString();
        //                legal.LegalName = r["TypeName"] == DBNull.Value ? "" : r["TypeName"].ToString();
        //                string date = r["DateModif"] == DBNull.Value ? r["DateCreate"].ToString() : r["DateModif"].ToString();
        //                if(date.CompareTo(lastDate) > 0 )
        //                {
        //                    lastDate = date;
        //                }
        //                legalLst.Add(legal);
        //            }
        //            foreach(LegalInfo legal in legalLst)
        //            {
        //                legal.LatestUpdate = lastDate; 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return legalLst;
        //}


    }
}
