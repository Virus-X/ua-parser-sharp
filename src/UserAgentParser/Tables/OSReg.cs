namespace UserAgentStringLibrary.Tables
{
    public class OSReg : UserAgentItem
    {
        public string RegString { get; private set; }

        public int OSID { get; private set; }

        public override int GetNumberItems()
        {
            return 2;
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
