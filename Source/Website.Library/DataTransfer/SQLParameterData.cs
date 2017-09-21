using System.Data;

namespace Website.Library.DataTransfer
{
    public class SQLParameterData
    {
        public object ParameterValue;
        public SqlDbType ParameterType;

        public SQLParameterData()
        {
        }

        public SQLParameterData(object parameterValue, SqlDbType parameterType = SqlDbType.VarChar)
        {
            ParameterValue = parameterValue;
            ParameterType = parameterType;
        }
    }
}