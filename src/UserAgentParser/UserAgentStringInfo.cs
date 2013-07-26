using System;

using UserAgentStringLibrary.Tables;

namespace UAParserSharp
{
    public class UserAgentStringInfo
    {
        bool isRobot = false;

        string ua_icon = "unknown.png";

        string ua_info_url = "unknown";

        string os_icon = "unknown.png";

        public string Type { get; private set; }

        public string UAFamily { get; private set; }

        public string UAName { get; private set; }

        public string UAUrl { get; private set; }

        public string UACompany { get; private set; }

        public string UACompanyUrl { get; private set; }

        public string OSFamily { get; private set; }

        public string OSName { get; private set; }

        public string OSUrl { get; private set; }

        public string OSCompany { get; private set; }

        public string OSCompanyUrl { get; private set; }

        public string UAIcon { get { return (new Uri(UASParser.UAImagesURL, ua_icon)).ToString(); } }

        public string UAInfoUrl { get { return (new Uri(UASParser.UserAgentStringURL, ua_info_url)).AbsoluteUri; } }

        public string OSIcon { get { return (new Uri(UASParser.OSImagesURL, os_icon)).ToString(); } }

        public UserAgentStringInfo()
        {
            OSCompanyUrl = "unknown";
            OSCompany = "unknown";
            OSUrl = "unknown";
            OSName = "unknown";
            OSFamily = "unknown";
            UACompanyUrl = "unknown";
            UACompany = "unknown";
            UAUrl = "unknown";
            UAName = "unknown";
            UAFamily = "unknown";
            Type = "unknown";
        }

        public UserAgentStringInfo(Robot robot, OS os)
        {
            Type = "Robot";
            UAName = robot.Name;
            UAFamily = robot.Family;
            UACompany = robot.Company;
            UAUrl = robot.URL;
            ua_info_url = robot.InfoURL;
            UACompanyUrl = robot.CompanyURL;
            ua_icon = robot.Icon;

            if (os != null)
            {
                OSCompany = os.Company;
                OSCompanyUrl = os.CompanyURL;
                OSFamily = os.Family;
                os_icon = os.Icon;
                OSName = os.Name;
                OSUrl = os.URL;
            }
        }

        public UserAgentStringInfo(Browser browser, string browserType, string version, OS os)
        {
            Type = browserType;
            UAFamily = browser.Name;
            UAName = browser.Name;
            UACompany = browser.Company;
            UAUrl = browser.URL;
            ua_info_url = browser.InfoURL;
            UACompanyUrl = browser.CompanyURL;
            ua_icon = browser.Icon;

            if (!string.IsNullOrEmpty(version))
            {
                UAName += " " + version;
            }

            if (os != null)
            {
                OSCompany = os.Company;
                OSCompanyUrl = os.CompanyURL;
                OSFamily = os.Family;
                os_icon = os.Icon;
                OSName = os.Name;
                OSUrl = os.URL;
            }
        }
    }
}
