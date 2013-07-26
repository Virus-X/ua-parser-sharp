namespace UserAgentStringLibrary.Tables
{
    public enum UserAgentType
    {
        Unknown = 0,
        Other = 1,
        Browser,
        OfflineBrowser,
        MobileBrowser,
        EmailClient,
        Library,
        WapBrowser,
        Validator,
        FeedReader,
        MultimediaPlayer,
        Anonymizer,
        Robot
    }

    public class BrowserType : UserAgentItem
    {
        public string TypeName { get; private set; }

        public UserAgentType Type { get; private set; }

        public override int GetNumberItems()
        {
            return 1;
        }

        protected override void LoadFields(string[] fields)
        {
            TypeName = fields[0];
            Type = ParseTypeName(TypeName);
        }

        private UserAgentType ParseTypeName(string typeName)
        {
            switch (typeName)
            {
                case "Browser":
                    return UserAgentType.Browser;
                case "Offline Browser":
                    return UserAgentType.OfflineBrowser;
                case "Mobile Browser":
                    return UserAgentType.MobileBrowser;
                case "Email client":
                    return UserAgentType.EmailClient;
                case "Library":
                    return UserAgentType.Library;
                case "Wap Browser":
                    return UserAgentType.WapBrowser;
                case "Validator":
                    return UserAgentType.Validator;
                case "Feed Reader":
                    return UserAgentType.FeedReader;
                case "Multimedia Player":
                    return UserAgentType.MultimediaPlayer;
                case "Useragent Anonymizer":
                    return UserAgentType.Anonymizer;
                case "Robot":
                    return UserAgentType.Robot;
                default:
                    return UserAgentType.Other;
            }
        }
    }
}
