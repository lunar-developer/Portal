using System.Configuration;
using LunarSoft.Library.Sql;
using Website.Library.Enum;

namespace Website.Library.DataAccess
{
    public abstract class DataProvider
    {
        protected readonly SqlServerConnector Connector;

        protected DataProvider() : this(ConnectionEnum.SiteModules)
        {
        }

        protected DataProvider(string connectionName)
        {
            Connector = new SqlServerConnector(
                ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
        }
    }
}