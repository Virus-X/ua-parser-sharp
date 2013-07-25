using System;
using System.Linq;

using UAParserSharp;

namespace UserAgentStringLibrary.Tables
{
  public class BrowserType : UserAgentItem
    {
        public string Type { get; set; }

        public override void Intialize(string s)
        {
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

          Type = x[0];
        }

        public BrowserType()
        {
        }

        public override ParserState GetState()
        {
          return ParserState.BrowserType;
        }

        public override int GetNumberItems()
        {
          return 1;
        }
    }
}
