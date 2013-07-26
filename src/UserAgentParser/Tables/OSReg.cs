using System.Text;
using System.Text.RegularExpressions;

using UserAgentParser.Util;

namespace UserAgentParser.Tables
{
    public class OSReg : UserAgentItem
    {
        private Regex regex;

        public string RegString { get; private set; }

        public int OSID { get; private set; }

        public override int GetNumberItems()
        {
            return 2;
        }

        public bool IsMatch(string userAgentString)
        {
            if (regex == null)
            {
                PerlRegExpConverter prec = new PerlRegExpConverter(RegString, null, Encoding.ASCII);
                regex = prec.Regex;
            }

            return regex.IsMatch(userAgentString);
        }

        protected override void LoadFields(string[] fields)
        {
            // [os_reg]--
            // os_reg_id[] = "OS regstring"
            // os_reg_id[] = "OS id"
            RegString = fields[0];
            int osid;
            if (int.TryParse(fields[1], out osid))
            {
                OSID = osid;
            }
        }
    }
}
