using System.Text;
using System.Text.RegularExpressions;

using UserAgentStringLibrary.Util;

namespace UserAgentStringLibrary.Tables
{
    public class BrowserReg : UserAgentItem
    {
        private Regex regex;

        public string RegString { get; private set; }

        public int BrowserID { get; private set; }

        public Regex Regex
        {
            get
            {
                if (regex == null)
                {
                    PerlRegExpConverter prec = new PerlRegExpConverter(RegString, null, Encoding.ASCII);
                    regex = prec.Regex;
                }

                return regex;
            }
        }

        public override int GetNumberItems()
        {
            return 2;
        }

        protected override void LoadFields(string[] fields)
        {
            // [browser_reg]--
            // browser_reg_id[] = "Browser regstring"
            // browser_reg_id[] = "Browser id"
            RegString = fields[0];
            int browserid;
            if (int.TryParse(fields[1], out browserid))
            {
                BrowserID = browserid;
            }
        }
    }
}
