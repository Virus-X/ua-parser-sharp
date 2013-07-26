namespace UserAgentStringLibrary.Tables
{
    public class BrowserType : UserAgentItem
    {
        public string Type { get; private set; }

        public override int GetNumberItems()
        {
            return 1;
        }

        protected override void LoadFields(string[] fields)
        {
            Type = fields[0];
        }
    }
}
