namespace UserAgentParser.Tests
{
    public class KnownUserAgent
    {
        public string UserAgentString { get; private set; }

        public string UserAgentName { get; private set; }

        public string OsName { get; private set; }

        public KnownUserAgent(string userAgentString, string userAgentName, string osName)
        {
            UserAgentString = userAgentString;
            UserAgentName = userAgentName;
            OsName = osName;
        }
    }
}