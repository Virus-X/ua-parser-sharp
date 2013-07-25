using System;
using System.Linq;

using UAParserSharp;

namespace UserAgentStringLibrary.Tables
{
  public class OS : UserAgentCommon
    {
        public string Family { get; set; }
        
        public OS()
        {
        }

        public override int GetNumberItems()
        {
          return 5+1;
        }

        public override void Intialize(string s)
        {
          //[os]--
          //os_id[] = "OS Family"
          //os_id[] = "OS Name"
          //os_id[] = "OS URL"
          //os_id[] = "OS Company"
          //os_id[] = "OS Company URL"
          //os_id[] = "OS ico"

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
                   select l.Split(new string[] {"[] ="}, StringSplitOptions.None)[1].Trim()).ToArray();

          Family = x[0];
          Name = x[1];
          URL = x[2];
          Company = x[3];
          CompanyURL = x[4];
          Icon = x[5];
        }

        public override ParserState GetState()
        {
          return ParserState.OS;
        }

    }
}
