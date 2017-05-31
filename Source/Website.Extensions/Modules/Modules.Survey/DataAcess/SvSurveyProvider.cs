using System;
using System.Collections.Generic;
using System.Data;
using Modules.Survey.Database;
using Website.Library.DataAccess;
using Modules.Survey.DataTransfer;
using Newtonsoft.Json;

namespace Modules.Survey.DataAcess
{
    class SvSurveyProvider : DataProvider
    {
        public int AddSurvey(SvSurveyData survey)
        {
            try
            {
                string sqlAddSurvey = $@"INSERT INTO {SvSurveyTable.TableNameRoot} ({SvSurveyTable.Survey},{SvSurveyTable.IsEnable}) VALUES(N'{survey.Survey}','1'); SELECT @@IDENTITY;";
                string idSurveyNew;
                Connector.ExecuteSql(sqlAddSurvey, out idSurveyNew);

                Connector.AddParameter("SurveyID",SqlDbType.Int,int.Parse(idSurveyNew));
                Connector.ExecuteProcedure("[dbo].[SV_SP_GetSurveyInfo]");
                return int.Parse(idSurveyNew);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int UpdateSurvey(SvSurveyData survey)
        {
            try
            {
                string sqlUpdateSurvey =
                    $@"UPDATE {SvSurveyTable.TableNameRoot} SET {SvSurveyTable.Survey}=N'{survey.Survey}',{SvSurveyTable.IsEnable}='{survey.IsEnable}' WHERE {SvSurveyTable
                        .Id} = {survey.Id}";
                Connector.ExecuteSql(sqlUpdateSurvey);

                Connector.AddParameter("SurveyID", SqlDbType.Int, int.Parse(survey.Id));
                Connector.ExecuteProcedure("[dbo].[SV_SP_GetSurveyInfo]");

                return int.Parse(survey.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public List<SurveyList> GetListSurvey()
        {
            try
            {
                string sqlGetListSurvey = $@"SELECT {SvSurveyTable.Id},{SvSurveyTable.Survey} FROM {SvSurveyTable.TableNameRoot};";
                List<SvSurveyData> sur;
                Connector.ExecuteSql<SvSurveyData, List<SvSurveyData>>(sqlGetListSurvey, out sur);
                if (sur == null)
                {
                    return null;
                }
                List<SurveyList> listSurvey = new List<SurveyList>();
                foreach (var item in sur)
                {
                    SurveyList listItem = new SurveyList
                    {
                        Id = int.Parse(item.Id),
                        Name = JsonConvert.DeserializeObject<RootSurvey>(item.Survey).pages[0].name
                    };
                    listSurvey.Add(listItem);
                }
                return listSurvey;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<SurveyList> GetListSurveyToAnswer()
        {
            try
            {
                string sqlGetListSurvey = $@"SELECT {SvSurveyTable.Id},{SvSurveyTable.Survey} FROM {SvSurveyTable.TableNameRoot} WHERE {SvSurveyTable.IsEnable}='1';";
                List<SvSurveyData> sur;
                Connector.ExecuteSql<SvSurveyData, List<SvSurveyData>>(sqlGetListSurvey, out sur);
                if (sur == null)
                {
                    return null;
                }
                List<SurveyList> listSurvey = new List<SurveyList>();
                foreach (var item in sur)
                {
                    SurveyList listItem = new SurveyList
                    {
                        Id = int.Parse(item.Id),
                        Name = JsonConvert.DeserializeObject<RootSurvey>(item.Survey).pages[0].name
                    };
                    listSurvey.Add(listItem);
                }
                return listSurvey;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public SvSurveyData FindSurvey(int id)
        {
            try
            {
                string sqlFindSurvey =
                    $@"SELECT {SvSurveyTable.Id},{SvSurveyTable.Survey},{SvSurveyTable.IsEnable} FROM {SvSurveyTable.TableNameRoot} WHERE {SvSurveyTable
                        .Id}={id}";
                SvSurveyData survey;
                Connector.ExecuteSql(sqlFindSurvey, out survey);
                return survey;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
