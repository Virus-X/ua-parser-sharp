

namespace UserAgentStringLibrary.Tables
{
    public class Browser : UserAgentCommon
    {
        public int TypeID { get; set; }

        public string InfoURL { get; set; }

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
