using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Survey.Database;
using Modules.Survey.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Survey.DataAcess
{
    class SvResultProvider : DataProvider
    {
        public int AddResult(int idSurvey, string RequestID, Dictionary<string, string> result)
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
                        listAdd.Add($@"('{idSurvey}','{RequestID}',N'{item.Key}',N'{item.Value[0].Replace("[", String.Empty).Replace("]", String.Empty)}','{DateTime.Now:yyyyMMddHHmmss}','')");
                    }
                    else
                    {
                        listAdd.Add($@"('{idSurvey}','{RequestID}',N'{item.Key}',N'{item.Value[0].Replace("[", String.Empty).Replace("]", String.Empty)}','{DateTime.Now:yyyyMMddHHmmss}',N'" + item.Value[1] + "')");
                    }
                    
                     
                    
                }
                sqlAdd += string.Join(",", listAdd.ToArray());
                return Connector.ExecuteSql(sqlAdd);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool CheckSurveyForClient(int idSurvey,string RequestID)
        {
            try
            {
                string sqlCheck = $@"SELECT * FROM {SvResultTable.TableNameRoot} WHERE {SvResultTable.IdSurvey} = {idSurvey} AND {SvResultTable.RequestID} = '{RequestID}';";
                List<SvResultData> rs; 
                Connector.ExecuteSql<SvResultData,List<SvResultData>>(sqlCheck,out rs);

                return rs.Count > 0;
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                return false;
            }
            
        }

        public Dictionary<string, List<string>> ConvertResultSurvey(Dictionary<string, string> dictionary)
        {
            IEnumerable<KeyValuePair<string, string>> ls = dictionary.Where(x => x.Key.EndsWith("-Comment"));
            Dictionary<string, string> lstemp = new Dictionary<string, string>();
            KeyValuePair<string, string>[] ls1 = ls.ToArray();


            foreach (var item in ls)
            {
                string key = item.Key.Replace("-Comment", String.Empty);
                if (dictionary.ContainsKey(key))
                {
                    lstemp.Add(key, item.Value);
                }
            }
            foreach (KeyValuePair<string, string> item in ls1)
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
