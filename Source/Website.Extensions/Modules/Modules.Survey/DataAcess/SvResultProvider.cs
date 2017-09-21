using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Survey.Database;
using Modules.Survey.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Survey.DataAcess
{
    internal class SvResultProvider : DataProvider
    {
        public int AddResult(int idSurvey, string requestID, Dictionary<string, string> result)
        {
            try
            {
                string sqlAdd = $@"INSERT INTO {SvResultTable.TableNameRoot}(
                                {SvResultTable.IdSurvey},
                                {SvResultTable.RequestID},
                                {SvResultTable.Question},
                                {SvResultTable.Result},
                                {SvResultTable.CreatedAt},
                                {SvResultTable.OtherResult}) VALUES";
                List<string> listAdd = new List<string>();

                foreach (var item in ConvertResultSurvey(result))
                {
                    if (item.Value.Count == 1)
                    {
                        listAdd.Add($@"('{idSurvey}','{requestID}',N'{item.Key}',N'{item.Value[0].Replace("[", string.Empty).Replace("]", string.Empty)}','{DateTime.Now:yyyyMMddHHmmss}','')");
                    }
                    else
                    {
                        listAdd.Add($@"('{idSurvey}','{requestID}',N'{item.Key}',N'{item.Value[0].Replace("[", string.Empty).Replace("]", string.Empty)}','{DateTime.Now:yyyyMMddHHmmss}',N'" + item.Value[1] + "')");
                    }
                    
                     
                    
                }
                sqlAdd += string.Join(",", listAdd.ToArray());
                return Connector.ExecuteSql(sqlAdd);
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckSurveyForClient(int idSurvey,string requestID)
        {
            try
            {
                string sqlCheck = $@"SELECT * FROM {SvResultTable.TableNameRoot} WHERE {SvResultTable.IdSurvey} = {idSurvey} AND {SvResultTable.RequestID} = '{requestID}';";
                List<SvResultData> rs; 
                Connector.ExecuteSql<SvResultData,List<SvResultData>>(sqlCheck,out rs);

                return rs.Count > 0;
            }
            catch
            {
                return false;
            }
           
        }

        public bool CheckSurveyIsEnabled(int idSurvey)
        {
            try
            {
                string sqlCheck = $@"SELECT * FROM {SvSurveyTable.TableNameRoot} WHERE {SvSurveyTable.Id} = {idSurvey} AND {SvSurveyTable.IsEnable} = 1;";
                SvSurveyData rs;
                Connector.ExecuteSql(sqlCheck, out rs);
                return rs != null;
            }
            catch
            {
                return false;
            }
            
        }

        public Dictionary<string, List<string>> ConvertResultSurvey(Dictionary<string, string> dictionary)
        {
            IEnumerable<KeyValuePair<string, string>> ls = dictionary.Where(x => x.Key.EndsWith("-Comment"));
            Dictionary<string, string> lstemp = new Dictionary<string, string>();
            IEnumerable<KeyValuePair<string, string>> keyValuePairs = ls as KeyValuePair<string, string>[] ?? ls.ToArray();
            KeyValuePair<string, string>[] pair = keyValuePairs.ToArray();


            foreach (var item in keyValuePairs)
            {
                string key = item.Key.Replace("-Comment", string.Empty);
                if (dictionary.ContainsKey(key))
                {
                    lstemp.Add(key, item.Value);
                }
            }
            foreach (KeyValuePair<string, string> item in pair)
            {
                dictionary.Remove(item.Key);
            }
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (var item in dictionary)
            {
                if (lstemp.ContainsKey(item.Key))
                {
                    List<string> listTemp = new List<string>()
                    {
                        item.Value,
                        lstemp[item.Key]
                    };
                    result.Add(item.Key, listTemp);
                }
                else
                {
                    List<string> listTemp = new List<string>()
                    {
                        item.Value
                    };
                    result.Add(item.Key, listTemp);
                }
            }

            return result;
        }
    }
}
