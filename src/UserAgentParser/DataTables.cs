using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UserAgentStringLibrary.Tables;

namespace UAParserSharp
{
    public enum ParserState
    {
        Browser,
        Robot,
        OS,
        BrowserOS,
        BrowserReg,
        BrowserType,
        OSReg,
        Unknown
    }

    public class DataTables
    {
        public static readonly Dictionary<int, Browser> Browsers = new Dictionary<int, Browser>();
        public static readonly Dictionary<int, BrowserOS> BrowserOss = new Dictionary<int, BrowserOS>();
        public static readonly Dictionary<int, BrowserReg> BrowserRegs = new Dictionary<int, BrowserReg>();
        public static readonly Dictionary<int, BrowserType> BrowserTypes = new Dictionary<int, BrowserType>();
        public static readonly Dictionary<int, OS> Oss = new Dictionary<int, OS>();
        public static readonly Dictionary<int, OSReg> OSRegs = new Dictionary<int, OSReg>();
        public static readonly Dictionary<int, Robot> Robots = new Dictionary<int, Robot>();

        private static ParserState GetState(string s)
        {
            switch (s)
            {
                case "[browser]":
                    return ParserState.Browser;
                case "[browser_type]":
                    return ParserState.BrowserType;
                case "[browser_reg]":
                    return ParserState.BrowserReg;
                case "[browser_os]":
                    return ParserState.BrowserOS;
                case "[os]":
                    return ParserState.OS;
                case "[os_reg]":
                    return ParserState.OSReg;
                case "[robots]":
                    return ParserState.Robot;
            }

            return ParserState.Unknown;
        }

        public static void LoadData(StringReader sr)
        {
            ParserState oldstate = ParserState.Unknown;
            ParserState newstate;
            StringBuilder sb = new StringBuilder();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                // comments
                if (line.StartsWith(";"))
                {
                    continue;
                }

                // table name
                if (line.StartsWith("["))
                {
                    newstate = GetState(line.Trim());

                    if (newstate != ParserState.Unknown)
                    {
                        if (oldstate != ParserState.Unknown && newstate != oldstate)
                        {
                            // LoadData Of Corresponding state
                            LoadTableData(oldstate, sb.ToString());
                            oldstate = newstate;
                            sb.Clear(); //.NET 4.0 method
                            //sb.Length = 0; //.NET 2.0+ method
                        }
                        else
                        {
                            oldstate = newstate;
                        }
                    }
                }
                else
                {
                    sb.AppendLine(line);
                }
            }

            if (sb.Length > 0)
            {
                LoadTableData(oldstate, sb.ToString());
            }
        }

        public static string[] Chop(string data, int lineNums)
        {
            List<string> list = new List<string>();
            StringReader sr = new StringReader(data);
            string line;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (i % lineNums == 0 && i != 0)
                {
                    list.Add(sb.ToString());
                    i = 0;
                    sb.Clear(); //.NET 4.0 method
                    //sb.Length = 0; //.NET 2.0+ method
                    i++;
                    sb.AppendLine(line);
                }
                else
                {
                    i++;
                    sb.AppendLine(line);
                }
            }

            if (!string.IsNullOrEmpty(sb.ToString().Trim()))
            {
                list.Add(sb.ToString());
            }

            return list.ToArray();
        }

        //This is ugly
        private static void LoadTableData(ParserState state, string data)
        {
            UserAgentItem uai;

            switch (state)
            {
                case ParserState.Robot:
                    uai = new Robot();
                    Robots.Clear();
                    break;
                case ParserState.Browser:
                    uai = new Browser();
                    Browsers.Clear();
                    break;
                case ParserState.OS:
                    uai = new OS();
                    Oss.Clear();
                    break;
                case ParserState.BrowserOS:
                    uai = new BrowserOS();
                    BrowserOss.Clear();
                    break;
                case ParserState.BrowserReg:
                    uai = new BrowserReg();
                    BrowserRegs.Clear();
                    break;
                case ParserState.BrowserType:
                    uai = new BrowserType();
                    BrowserTypes.Clear();
                    break;
                case ParserState.OSReg:
                    uai = new OSReg();
                    OSRegs.Clear();
                    break;
                default:
                    return;
            }

            var dataSpliced = Chop(data, uai.GetNumberItems());

            // Figure out what to create....... 
            foreach (string d in dataSpliced)
            {
                switch (state)
                {
                    case ParserState.Robot:
                        uai = new Robot();
                        uai.Intialize(d);
                        Robots.Add(uai.ID, (Robot)uai);
                        break;
                    case ParserState.Browser:
                        uai = new Browser();
                        uai.Intialize(d);
                        Browsers.Add(uai.ID, (Browser)uai);
                        break;
                    case ParserState.OS:
                        uai = new OS();
                        uai.Intialize(d);
                        Oss.Add(uai.ID, (OS)uai);
                        break;
                    case ParserState.BrowserOS:
                        uai = new BrowserOS();
                        uai.Intialize(d);
                        BrowserOss.Add(uai.ID, (BrowserOS)uai);
                        break;
                    case ParserState.BrowserReg:
                        uai = new BrowserReg();
                        uai.Intialize(d);
                        BrowserRegs.Add(uai.ID, (BrowserReg)uai);
                        break;
                    case ParserState.BrowserType:
                        uai = new BrowserType();
                        uai.Intialize(d);
                        BrowserTypes.Add(uai.ID, (BrowserType)uai);
                        break;
                    case ParserState.OSReg:
                        uai = new OSReg();
                        uai.Intialize(d);
                        OSRegs.Add(uai.ID, (OSReg)uai);
                        break;
                    default:
                        return;
                }
            }
        }

        public void LoadData(string s)
        {
            LoadData(new StringReader(s));
        }
    }
}
