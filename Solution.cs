using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    public class Solution
    {
        public string Name { get; set; }

        public List<Rule> Rules { get; set; }

    }

    public class Rule
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsRRule { get; set; }

        public string InFileName => IsRRule ? string.Join("_", Name, Value) : Name;
    }
}
