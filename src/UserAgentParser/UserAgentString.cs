namespace UserAgentParser
{
    public class UserAgentString
    {
        public string Raw { get; private set; }

        public UserAgentInfo UserAgent { get; private set; }

        public OsInfo OS { get; private set; }

        public UserAgentString()
        {
            Raw = string.Empty;
            UserAgent = UserAgentInfo.Unknown;
            OS = OsInfo.Unknown;
        }

        public UserAgentString(string raw, UserAgentInfo userAgent, OsInfo os)
        {
            Raw = raw;
            UserAgent = userAgent ?? UserAgentInfo.Unknown;
            OS = os ?? OsInfo.Unknown;
        }
    }
}
