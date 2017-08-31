using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Library.Enum;

namespace Modules.VSaleKit.DataAccess
{
    public class DatabaseCtrl
    {
        protected SQLAccess dbVS;
        public DatabaseCtrl(string connectionName)
        {
            this.dbVS = new SQLAccess(GetConnectionSetting(connectionName));            
        }
        private static string GetConnectionSetting(string name)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[name];
            if (setting != null)
            {
                return setting.ConnectionString;
            }
            throw new KeyNotFoundException("Can not found connection :" + name);
        }

        public void Dispose()
        {
            if (this.dbVS != null)
            {
                this.dbVS.Dispose();
            }           
        }
    }
}
