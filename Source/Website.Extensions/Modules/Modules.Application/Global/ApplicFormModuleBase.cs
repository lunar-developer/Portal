using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Website.Library.Global;

namespace Modules.Application.Global
{
    public partial class ApplicationModuleBase
    {
        protected static string GetCountryCodeByID(string id)
        {
            return CacheBase.Receive<CountryData>(id).CountryCode;
        }
        

        protected static string GetStateCodeByID(string id)
        {
            return CacheBase.Receive<StateData>(id).StateCode;
        }
        protected static void BindStateData(DropDownList dropDownList,string countryID, 
            string selectedValue)
        {
            if (!string.IsNullOrWhiteSpace(countryID))
            {
                string countryCode = GetCountryCodeByID(countryID);
                List<StateData> dataList = CacheBase.Receive<StateData>();
                foreach (StateData data in dataList)
                {
                    ListItem item = new ListItem(data.StateName, data.StateCode)
                    {
                        Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
                            (data.StateCode.Equals(selectedValue) || data.StateCode.Equals(selectedValue))
                    };
                    if (!string.IsNullOrWhiteSpace(countryCode) && 
                        countryCode.Equals(data.CountryCode))
                    {
                        dropDownList.Items.Add(item);
                    }
                }
            }
            
        }

        public static string GetCityCodeByID(string id)
        {
            return CacheBase.Receive<CityData>(id).CityCode;
        }
        protected static void BindCityData(DropDownList dropDownList, string stateID, string selectedValue)
        {
            if (!string.IsNullOrWhiteSpace(stateID))
            {
                string stateCode = GetStateCodeByID(stateID);
                List<CityData> dataList = CacheBase.Receive<CityData>();
                foreach (CityData data in dataList)
                {
                    ListItem item = new ListItem(data.CityName, data.CityCode)
                    {
                        Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
                            (data.CityCode.Equals(selectedValue) || data.CityCode.Equals(selectedValue))
                    };
                    if (!string.IsNullOrWhiteSpace(stateCode) && stateCode.Equals(data.StateCode))
                    {
                        dropDownList.Items.Add(item);
                    }
                }
            }
        }


        protected static string GetSqlInsertApplicField(List<string> sqList)
        {
            string sql = string.Empty;
            foreach (string str in sqList)
            {
                sql += str;
            }
            return sql;
        }


    }
}
