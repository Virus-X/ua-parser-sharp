using System;

namespace UserAgentParser.Tables
{
    public class OS : UserAgentCommon
    {
        public static readonly Uri UnknownIconUrl = new Uri(Constants.OSImagesUrl, "unknown.png");

        public string Family { get; private set; }

        public Uri IconUrl
        {
            get
            {
                return string.IsNullOrEmpty(Icon) ? UnknownIconUrl : new Uri(Constants.OSImagesUrl, Icon);
            }
        }

        public override int GetNumberItems()
        {
            return 5 + 1;
        }

        protected override void LoadFields(string[] fields)
        {
            // [os]--
            // os_id[] = "OS Family"
            // os_id[] = "OS Name"
            // os_id[] = "OS URL"
            // os_id[] = "OS Company"
            // os_id[] = "OS Company URL"
            // os_id[] = "OS ico"
            Family = fields[0];
            Name = fields[1];
            URL = fields[2];
            Company = fields[3];
            CompanyURL = fields[4];
            Icon = fields[5];
        }
    }
}
