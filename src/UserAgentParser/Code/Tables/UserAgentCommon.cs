using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAParserSharp
{
    public abstract class UserAgentCommon : UserAgentItem
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Company { get; set; }
        public string CompanyURL { get; set; }
        public string Icon { get; set; }


    }
}
