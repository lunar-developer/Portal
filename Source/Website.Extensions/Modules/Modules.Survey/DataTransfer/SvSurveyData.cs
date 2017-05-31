using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Survey.DataTransfer
{
    public class SvSurveyData
    {
        public string Id;
        public string Survey;
        public string IsEnable;
    }

    public class Item
    {
        public string name { get; set; }
    }

    public class Element
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<Item> items { get; set; }
        public List<object> choices { get; set; }
        public List<object> columns { get; set; }
        public List<string> rows { get; set; }
    }

    public class Page
    {
        public string name { get; set; }
        public List<Element> elements { get; set; }
    }

    public class RootSurvey
    {
        public List<Page> pages { get; set; }
    }

    public class SurveyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
