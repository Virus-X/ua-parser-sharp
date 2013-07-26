using System;

using UserAgentStringLibrary.Tables;

namespace UAParserSharp
{
    public class UserAgentInfo
    {
        public string Type { get; set; }

        public string Family { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Version { get; set; }

        public string Company { get; set; }

        public string CompanyUrl { get; set; }

        public Uri IconUrl { get; set; }

        public Uri InfoUrl { get; set; }

        public static UserAgentInfo Unknown
        {
            get
            {
                return new UserAgentInfo
                {
                    Name = "Unknown",
                    Family = "Unknown",
                    Company = "Unknown",
                    Type = "Unknown",
                    IconUrl = Browser.UnknownIconUrl
                };
            }
        }

        public UserAgentInfo()
        {
        }

        public UserAgentInfo(Robot robot)
        {
            Type = "Robot";
            Name = robot.Name;
            Family = robot.Family;
            Company = robot.Company;
            Url = robot.URL;
            IconUrl = robot.IconUrl;
            CompanyUrl = robot.CompanyURL;
            InfoUrl = robot.InfoUrl;
        }

        public UserAgentInfo(Browser browser, string browserType, string version)
        {
            Type = browserType;
            Version = version;
            Name = browser.Name;
            Family = browser.Name;
            Company = browser.Company;
            Url = browser.URL;
            IconUrl = browser.IconUrl;
            CompanyUrl = browser.CompanyURL;
            InfoUrl = browser.InfoUrl;

            if (!string.IsNullOrEmpty(version))
            {
                Name += " " + version;
            }
        }
    }
}