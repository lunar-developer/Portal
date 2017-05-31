using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Survey.DataAcess;

namespace Modules.Survey.Business
{
    public class SvResultBusiness
    {
        public int AddResult(int idSurvey, string RequestID, Dictionary<string, string> result)
            => new SvResultProvider().AddResult(idSurvey, RequestID, result);

        public bool CheckSurveyForClient(int idSurvey, string RequestID)
            => new SvResultProvider().CheckSurveyForClient(idSurvey, RequestID);

        public bool CheckSurveyIsEnabled(int idSurvey)
            => new SvResultProvider().CheckSurveyIsEnabled(idSurvey);
    }
}
