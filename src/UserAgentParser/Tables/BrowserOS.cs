namespace UserAgentParser.Tables
{
    public class BrowserOS : UserAgentItem
    {
        public int OSID { get; private set; }

        public override int GetNumberItems()
        {
            return 1 + 1;
        }

        protected override void LoadFields(string[] fields)
        {
            // [browser_os]--
            // browser_id[] = "OS id";
            int osid;
            if (int.TryParse(fields[0], out osid))
            {
                OSID = osid;
            }
        }
    }
}
