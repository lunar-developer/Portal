using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace Modules.VSaleKit.DataAccess
{
    public class SQLAccess : IDisposable
    {
        protected SqlConnection conn;
        public SqlConnection Connection
        {
            get
            {
                return this.conn;
            }
        }
        /// <summary>
        /// database
        /// </summary>

        public SQLAccess(String connectiontString)
        {
            this.conn = new SqlConnection();
            this.conn.ConnectionString = connectiontString;
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (this.conn.State == ConnectionState.Open)
                    {
                        this.conn.Close();
                    }
                    this.conn.Dispose();
                    this.conn = null;
                }
            }
            catch (Exception ex)
            {
                //Vsalekit.Library.Logs.LogManager.LogError(ex.ToString());
                throw ex;
            }
        }


    }
    public interface ISQLAccess
    {
        SqlConnection Connection
        {
            set;
        }
    }
    public class SQLParameterCtr
    {
        public SqlParameter CreateParameter(string name, SqlDbType ptype, ParameterDirection direct, object value)
        {
            SqlParameter para = new SqlParameter();
            para.SqlDbType = ptype;
            para.ParameterName = name;
            if (value != null)
            {
                para.Value = value;
            }
            else
            {
                para.Value = System.DBNull.Value;
            }
            para.Direction = direct;
            return para;
        }

        public SqlParameter CreateParameter(string name, SqlDbType ptype, ParameterDirection direct, object value, int size)
        {
            SqlParameter para = new SqlParameter();
            para.SqlDbType = ptype;
            para.ParameterName = name;
            para.Size = size;

            if (value != null)
            {
                para.Value = value;
            }
            else
            {
                para.Value = System.DBNull.Value;
            }

            para.Direction = direct;
            return para;
        }

        public SqlParameter CreateParameter(string name, SqlDbType ptype, ParameterDirection direct)
        {
            SqlParameter para = new SqlParameter();
            para.SqlDbType = ptype;
            para.ParameterName = name;
            para.Direction = direct;
            return para;
        }
        public SqlParameter CreateParameterWithSize(string name, SqlDbType ptype, ParameterDirection direct, int size)
        {
            SqlParameter para = new SqlParameter();
            para.SqlDbType = ptype;
            para.ParameterName = name;
            para.Direction = direct;
            para.Size = size;
            return para;
        }
        public SqlCommand CreateCommand(string sql, SqlConnection connection, CommandType ptype, int timeout)
        {
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = ptype;
            cmd.CommandTimeout = timeout;
            return cmd;
        }
    }
}
