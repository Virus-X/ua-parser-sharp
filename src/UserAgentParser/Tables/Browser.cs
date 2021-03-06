﻿using System;

namespace UserAgentParser.Tables
{
    public class Browser : UserAgentCommon
    {
        public static readonly Uri UnknownIconUrl = new Uri(Constants.UaImagesUrl, "unknown.png");

        public int TypeID { get; private set; }

        public string InfoURL { get; private set; }

        public Uri IconUrl
        {
            get
            {
                return string.IsNullOrEmpty(Icon) ? UnknownIconUrl : new Uri(Constants.UaImagesUrl, Icon);
            }
        }

        public Uri InfoUrl
        {
            get { return new Uri(Constants.UserAgentStringUrl, string.IsNullOrEmpty(InfoURL) ? "unknown" : InfoURL); }
        }

        public override int GetNumberItems()
        {
            return 5 + 2;
        }

        protected override void LoadFields(string[] fields)
        {
            // [browser]--
            // browser_id[] = "Browser type"
            // browser_id[] = "Browser Name"
            // browser_id[] = "Browser URL"
            // browser_id[] = "Browser Company"
            // browser_id[] = "Browser Company URL"
            // browser_id[] = "Browser ico" 
            // browser_id[] = "Browser info URL"
            int type;
            if (int.TryParse(fields[0], out type))
            {
                TypeID = type;
            }

            Name = fields[1];
            URL = fields[2];
            Company = fields[3];
            CompanyURL = fields[4];
            Icon = fields[5];
            InfoURL = fields[6];
        }
    }
}
