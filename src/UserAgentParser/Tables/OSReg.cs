using System;
using System.Linq;

using UAParserSharp;

namespace UserAgentStringLibrary.Tables
{
  public class OSReg : UserAgentItem
    {
        public string RegString { get; set; }
        public int OSID { get; set; }

        public override void Intialize(string s)
        {
          //[os_reg]--
          //os_reg_id[] = "OS regstring"
          //os_reg_id[] = "OS id"

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
          int osid;
          if (int.TryParse(x[1], out osid))
            OSID = osid;
        }

        public override ParserState GetState()
        {
          return ParserState.OSReg;
        }

        public override int GetNumberItems()
        {
          return 2;
        }
    }
}
