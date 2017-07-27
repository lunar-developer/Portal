using System;
using System.Collections.Generic;

namespace Website.Library.Extension
{
    public class InsensitiveList : HashSet<string>
    {
        public InsensitiveList() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public InsensitiveList(IEnumerable<string> list) : base(list, StringComparer.OrdinalIgnoreCase)
        {
        }


        public bool Append(string value)
        {
            return Contains(value) == false && Add(value);
        }
    }
}