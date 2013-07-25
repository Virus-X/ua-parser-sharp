using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAParserSharp
{
    public class Robot : UserAgentCommon
    {
        public string UserAgentString { get; set; }
        public string Family { get; set; }
        public string OsID { get; set; }
        public string InfoURL { get; set; }

        public override void Intialize(string s)
        {
          //; bot_id[] = "bot useragentstring"
          //; bot_id[] = "bot Family"
          //; bot_id[] = "bot Name"
          //; bot_id[] = "bot URL"
          //; bot_id[] = "bot Company"
          //; bot_id[] = "bot Company URL"
          //; bot_id[] = "bot ico"
          //; bot_id[] = "bot OS id"
          //; bot_id[] = "bot info URL"
          string[] lines = s.Trim().Replace("\r","").Replace("\"","").Split(new char[] {'\n'});

          //getID
          string sID = lines[0].Split('=')[0].Trim().Replace("[]","");
          int id;
          if (!int.TryParse(sID, out id))
            return;
          else
            ID = id;

          //getOther items
          var x = (from l in lines
                  select l.Split(new string[] {"[] ="}, StringSplitOptions.None)[1].Trim()).ToArray();

          UserAgentString = x[0];
          Family = x[1];
          Name = x[2];
          URL = x[3];
          Company = x[4];
          CompanyURL = x[5];
          Icon = x[6];
          OsID = x[7];
          InfoURL = x[8];

        }  

        public override int GetNumberItems()
        {
          return 4+5;
        }

        public override State GetState()
        {
          return State.Robot;
        }

        public Robot()
        {
        }
    }
}
