using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAParserSharp
{
    public class BrowserOS: UserAgentItem
    {
        public int OSID { get; set; }

        public BrowserOS()
        {
        }

        public override void Intialize(string s)
        {
          //[browser_os]--
          //browser_id[] = "OS id"

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

          int osid;
          if (int.TryParse(x[0], out osid))
            OSID = osid;
        }

        public override State GetState()
        {
          return State.BrowserOS;
        }

        public override int GetNumberItems()
        {
          return 1+1;
        }
    }
}
