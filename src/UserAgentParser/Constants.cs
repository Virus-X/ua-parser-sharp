using System;

namespace UserAgentParser
{
    public static class Constants
    {
        public static readonly string VersionUrl = @"http://user-agent-string.info/rpc/get_data.php?key=free&format=ini&ver=y";

        public static readonly string IniFileUrl = @"http://user-agent-string.info/rpc/get_data.php?key=free&format=ini";

        public static readonly Uri UaImagesUrl = new Uri(@"http://user-agent-string.info/pub/img/ua/");

        public static readonly Uri OSImagesUrl = new Uri(@"http://user-agent-string.info/pub/img/os/");

        public static readonly Uri UserAgentStringUrl = new Uri(@"http://user-agent-string.info/");
    }
}