using System;

namespace UserAgentParser.Tables
{
    public class Robot : UserAgentCommon
    {
        public static readonly Uri UnknownIconUrl = new Uri(Constants.UaImagesUrl, "unknown.png");

        public string UserAgentString { get; private set; }

        public string Family { get; private set; }

        public int? OsID { get; private set; }

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
            return 4 + 5;
        }

        protected override void LoadFields(string[] fields)
        {
            // bot_id[] = "bot useragentstring"
            // bot_id[] = "bot Family"
            // bot_id[] = "bot Name"
            // bot_id[] = "bot URL"
            // bot_id[] = "bot Company"
            // bot_id[] = "bot Company URL"
            // bot_id[] = "bot ico"
            // bot_id[] = "bot OS id"
            // bot_id[] = "bot info URL";
            UserAgentString = fields[0];
            Family = fields[1];
            Name = fields[2];
            URL = fields[3];
            Company = fields[4];
            CompanyURL = fields[5];
            Icon = fields[6];


            int osid;
            if (int.TryParse(fields[7], out osid))
            {
                OsID = osid;
            }

            InfoURL = fields[8];
        }
    }
}
