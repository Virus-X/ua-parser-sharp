using System;

using UserAgentParser.Tables;

namespace UserAgentParser
{
    public class OsInfo
    {
        public string Family { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Company { get; set; }

        public string CompanyUrl { get; set; }

        public Uri IconUrl { get; set; }

        public static OsInfo Unknown
        {
            get
            {
                return new OsInfo
                    {
                        Name = "Unknown",
                        Family = "Unknown",
                        Company = "Unknown",
                        IconUrl = OS.UnknownIconUrl
                    };
            }
        }

        public OsInfo()
        {
        }

        public OsInfo(OS os)
        {
            Company = os.Company;
            CompanyUrl = os.CompanyURL;
            Family = os.Family;
            IconUrl = os.IconUrl;
            Name = os.Name;
            Url = os.URL;
        }
    }
}