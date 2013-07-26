using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UAParserSharp;

namespace UserAgentParser.Tests
{
    [TestFixture]
    public class UASParserTests
    {
        private static readonly List<KnownUserAgent> KnownUserAgents = new List<KnownUserAgent>
            {
                new KnownUserAgent(
                    "Mozilla/5.0 (SAMSUNG; SAMSUNG-GT-S8500/S8500XXJF4; U; Bada/1.0; fr-fr) AppleWebKit/533.1 (KHTML, like Gecko) Dolfin/2.0 Mobile WVGA SMM-MMS/1.2.0 OPN-B", 
                    "Dolphin 2.0", 
                    "Bada"),
                new KnownUserAgent(
                    "Mozilla/5.0 (Windows NT 5.1; rv:13.0) Gecko/20100101 Firefox/13.0.1",
                    "Firefox 13.0.1",
                    "Windows XP")
            };

        private IEnumerable<TestCaseData> userAgentNames = KnownUserAgents.Select(x => new TestCaseData(x.UserAgentString, x.UserAgentName));
        private IEnumerable<TestCaseData> userAgentOperatingSystems = KnownUserAgents.Select(x => new TestCaseData(x.UserAgentString, x.OsName));

        private UASParser parser;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            parser = new UASParser(".");
            parser.GetLatestDataFile();
        }

        [Test]
        [TestCaseSource("userAgentNames")]
        public void Parse_KnownString_UserAgentValid(string uas, string expectedUserAgentName)
        {
            var res = parser.Parse(uas);
            Assert.AreEqual(expectedUserAgentName, res.UAName);
        }

        [Test]
        [TestCaseSource("userAgentOperatingSystems")]
        public void Parse_KnownString_OsNameValid(string uas, string expectedOsName)
        {
            var res = parser.Parse(uas);
            Assert.AreEqual(expectedOsName, res.OSName);
        }
    }
}
