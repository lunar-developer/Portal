using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Survey.DataAcess;
using Modules.Survey.DataTransfer;

namespace Modules.Survey.Business
{
    public class SvSurveyBusiness
    {
        public int AddSurvey(SvSurveyData survey) => new SvSurveyProvider().AddSurvey(survey);
        public List<SurveyList> GetListSurvey() => new SvSurveyProvider().GetListSurvey();
        public SvSurveyData FindSurvey(int id) => new SvSurveyProvider().FindSurvey(id);
        public int UpdateSurvey(SvSurveyData survey) => new SvSurveyProvider().UpdateSurvey(survey);
        public List<SurveyList> GetListSurveyToAnswer() => new SvSurveyProvider().GetListSurveyToAnswer();
    }
}
