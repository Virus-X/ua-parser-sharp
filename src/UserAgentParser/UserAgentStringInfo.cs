using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UserAgentStringLibrary.Tables;
using UserAgentStringLibrary.Util;

namespace UAParserSharp
{    
    public class UserAgentStringInfo
    {
        bool isRobot = false;

        string type = "unknown";
        string ua_family = "unknown";
        string ua_name = "unknown";
        string ua_url = "unknown";
        string ua_company = "unknown";
        string ua_company_url = "unknown";
        string ua_icon = "unknown.png";
        string ua_info_url = "unknown";
        string os_family = "unknown";
        string os_name = "unknown";
        string os_url = "unknown";
        string os_company = "unknown";
        string os_company_url = "unknown";
        string os_icon = "unknown.png";

        public string Type { get { return type; } }
        public string UAFamily { get { return ua_family; } }
        public string UAName { get { return ua_name; } }
        public string UAUrl { get { return ua_url; } }
        public string UACompany { get { return ua_company; } }
        public string UACompanyUrl { get { return ua_company_url; } }
        public string UAIcon { get { return (new Uri(UASParser.UAImagesURL, ua_icon)).ToString(); } }
        public string UAInfoUrl { get { return (new Uri(UASParser.UserAgentStringURL, ua_info_url)).AbsoluteUri; } }
        public string OSFamily { get { return os_family; } }
        public string OSName { get { return os_name; } }
        public string OSUrl { get { return os_url; } }
        public string OSCompany { get { return os_company; } }
        public string OSCompanyUrl { get { return os_company_url; } }
        public string OSIcon { get { return (new Uri(UASParser.OSImagesURL, os_icon)).ToString(); } }


        public UserAgentStringInfo()
        {

        }

        public void Parse(string uas, DataTables dts)
        {
            //First Robot
            Robot robot = null;
            foreach (Robot r in DataTables.robots.Values)
            {
                if (uas.CompareTo(r.UserAgentString) == 0)
                {
                    robot = r;
                    break;
                }
            }

            if (robot != null)
            {
                isRobot = true;
                type = "Robot";
                ua_name = robot.Name;
                ua_family = robot.Family;
                ua_company = robot.Company;
                ua_url = robot.URL;
                ua_info_url = robot.InfoURL;
                ua_company_url = robot.CompanyURL;
                ua_icon = robot.Icon;

                //OSINFO for Robot
                if (robot.OsID != "" && robot.OsID != null)
                {
                    int osid;
                    if (int.TryParse(robot.OsID, out osid))
                    {
                        OS os = DataTables.oss[osid];
                        os_company = os.Company;
                        os_company_url = os.CompanyURL;
                        os_family = os.Family;
                        os_icon = os.Icon;
                        os_name = os.Name;
                        os_url = os.URL;
                    }
                }


            }
            else //ELSE ->
            {

                //BrowserReg
                Browser browser = null;
                foreach (KeyValuePair<int, BrowserReg> br in DataTables.browserRegs)
                {
                    try
                    {
                        PerlRegExpConverter prec = new PerlRegExpConverter(br.Value.RegString, null, Encoding.ASCII);
                        Regex r = prec.Regex;
                        //Regex r = new Regex(br.Value.RegString);
                        Match m = r.Match(uas);
                        if (m.Success)
                        {
                            GroupCollection gc = m.Groups;

                            browser = DataTables.browsers[br.Value.BrowserID];
                            foreach (Group g in gc)
                            {
                                string v = null;
                                double version;
                                if (double.TryParse(g.Value.Replace(".", ""), out version))
                                    v = g.Value;

                                if (v != null)
                                {

                                    ua_name = " " + v;
                                    break;
                                }
                                else
                                    ua_name = "";
                            }

                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        int i = 0;
                        i++;
                    }
                }

                if (browser != null)
                {
                    int t = browser.TypeID;
                    type = DataTables.browserTypes[t].Type;
                    if (ua_name.CompareTo("unknown") == 0)
                        ua_name = browser.Name;
                    else
                        ua_name = browser.Name + ua_name;
                    ua_family = browser.Name;
                    ua_company = browser.Company;
                    ua_url = browser.URL;
                    ua_info_url = browser.InfoURL;
                    ua_company_url = browser.CompanyURL;
                    ua_icon = browser.Icon;

                    OS os = null;
                    //Os - first BrowserOS, 
                    if (DataTables.browserOss.ContainsKey(browser.ID))
                    {
                        os = DataTables.oss[DataTables.browserOss[browser.ID].OSID];
                    }
                    else //else Os regexp
                    {
                        foreach (KeyValuePair<int, OSReg> osr in DataTables.osRegs)
                        {
                            try
                            {
                                PerlRegExpConverter prec = new PerlRegExpConverter(osr.Value.RegString, null, Encoding.ASCII);
                                Regex r = prec.Regex;
                                if (r.IsMatch(uas))
                                {
                                    os = DataTables.oss[osr.Value.OSID];
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                int i = 0;
                                i++;
                            }
                        }
                    }

                    if (os != null)
                    {
                        os_company = os.Company;
                        os_company_url = os.CompanyURL;
                        os_family = os.Family;
                        os_icon = os.Icon;
                        os_name = os.Name;
                        os_url = os.URL;
                    }




                }
            }
        }

    }
}
