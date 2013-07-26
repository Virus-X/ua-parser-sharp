namespace UserAgentParser.Tables
{
    public abstract class UserAgentCommon : UserAgentItem
    {
        public string Name { get; protected set; }

        public string URL { get; protected set; }

        public string Company { get; protected set; }

        public string CompanyURL { get; protected set; }

        public string Icon { get; protected set; }
    }
}
