using System;
using System.Linq;

using UAParserSharp;

namespace UserAgentStringLibrary.Tables
{
    public class Browser : UserAgentCommon
    {
        public int TypeID { get; set; }
        public string InfoURL { get; set; }

        public Browser()
        {
        }

        public override void Intialize(string s)
        {
            //[browser]--
            //browser_id[] = "Browser type"
            //browser_id[] = "Browser Name"
            //browser_id[] = "Browser URL"
            //browser_id[] = "Browser Company"
            //browser_id[] = "Browser Company URL"
            //browser_id[] = "Browser ico" 
            //browser_id[] = "Browser info URL"

            string[] lines = s.Trim().Replace("\r", "").Replace("\"", "").Split(new char[] { '\n' });

            //getID
            string sID = lines[0].Split('=')[0].Trim().Replace("[]", "");
            int id;
            if (!int.TryParse(sID, out id))
                return;
            else
                ID = id;

            //getOther items
            var x = (from l in lines
                     select l.Split(new string[] { "[] =" }, StringSplitOptions.None)[1].Trim()).ToArray();

            int type;
            if (int.TryParse(x[0], out type))
                TypeID = type;

            Name = x[1];
            URL = x[2];
            Company = x[3];
            CompanyURL = x[4];
            Icon = x[5];
            InfoURL = x[6];
        }

        public override ParserState GetState()
        {
            return ParserState.Browser;
        }

        public override int GetNumberItems()
        {
            return 5 + 2;
        }
    }
}
