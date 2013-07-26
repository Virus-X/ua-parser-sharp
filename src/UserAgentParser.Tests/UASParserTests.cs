using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UAParserSharp;

using UserAgentStringLibrary.Tables;

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
        }

        [Test]
        [TestCaseSource("userAgentNames")]
        public void Parse_KnownString_UserAgentValid(string uas, string expectedUserAgentName)
        {
            var res = parser.Parse(uas);
            Assert.AreEqual(expectedUserAgentName, res.UserAgent.Name);
        }

        [Test]
        public void Parse_KnownBrowserString_UserAgentTypeValid()
        {
            var res = parser.Parse("Mozilla/5.0 (Windows NT 5.1; rv:13.0) Gecko/20100101 Firefox/13.0.1");
            Assert.AreEqual(UserAgentType.Browser, res.UserAgent.Type);
        }

        [Test]
        public void Parse_KnownMailClientString_UserAgentTypeValid()
        {
            var res = parser.Parse("Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.8) Gecko/20100802 Lightning/1.0b2 Thunderbird/3.1.2 ThunderBrowse/3.3.2");
            Assert.AreEqual(UserAgentType.EmailClient, res.UserAgent.Type);
        }

        [Test]
        [TestCaseSource("userAgentOperatingSystems")]
        public void Parse_KnownString_OsNameValid(string uas, string expectedOsName)
        {
            var res = parser.Parse(uas);
            Assert.AreEqual(expectedOsName, res.OS.Name);
        }

        [Test]
        public void Parse_UnknownString_OsNameUnknown()
        {
            var res = parser.Parse("???");
            Assert.AreEqual("Unknown", res.OS.Name);
        }

        [Test]
        public void Parse_UnknownString_UserAgentNameUnknown()
        {
            var res = parser.Parse("???");
            Assert.AreEqual("Unknown", res.UserAgent.Name);
        }

        [Test]
        public void Parse_UnknownString_UserAgentIconPointsToUnknownPng()
        {
            var res = parser.Parse("???");
            Assert.AreEqual(new Uri(UASParser.UAImagesURL, "unknown.png"), res.UserAgent.IconUrl);
        }

        [Test]
        public void Parse_UnknownString_OsIconPointsToUnknownPng()
        {
            var res = parser.Parse("???");
            Assert.AreEqual(new Uri(UASParser.OSImagesURL, "unknown.png"), res.OS.IconUrl);
        }
    }
}
