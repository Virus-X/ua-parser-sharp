using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAParserSharp
{
  public class BrowserReg : UserAgentItem
    {
        public string RegString { get; set; }
        public int BrowserID { get; set; }

        public BrowserReg()
        {
        }

        public override void Intialize(string s)
        {
          //[browser_reg]--
          //browser_reg_id[] = "Browser regstring"
          //browser_reg_id[] = "Browser id"

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

          RegString = x[0];
          int browserid;
          if (int.TryParse(x[1], out browserid))
            BrowserID = browserid;

          
        }
        public override State GetState()
        {
          return State.BrowserReg;
        }

        public override int GetNumberItems()
        {
          return 2;
        }
    }
}
