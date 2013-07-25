﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        public DateTime Created { get; set; }

        public DataTables()
        {
            Created = DateTime.Now;
        }

        public static Dictionary<int, Browser> browsers = new Dictionary<int, Browser>();
        public static Dictionary<int, BrowserOS> browserOss = new Dictionary<int, BrowserOS>();
        public static Dictionary<int, BrowserReg> browserRegs = new Dictionary<int, BrowserReg>();
        public static Dictionary<int, BrowserType> browserTypes = new Dictionary<int, BrowserType>();
        public static Dictionary<int, OS> oss = new Dictionary<int, OS>();
        public static Dictionary<int, OSReg> osRegs = new Dictionary<int, OSReg>();
        public static Dictionary<int, Robot> robots = new Dictionary<int, Robot>();

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
                //comments
                if (line.StartsWith(";"))
                    continue;
                //table name
                if (line.StartsWith("["))
                {
                    newstate = GetState(line.Trim());
                    //
                    if (newstate != ParserState.Unknown)
                        if (oldstate != ParserState.Unknown && newstate != oldstate)
                        {
                            //LoadData Of Corresponding state
                            CreateDictionary(oldstate, sb.ToString());
                            oldstate = newstate;
                            sb.Clear(); //.NET 4.0 method
                            //sb.Length = 0; //.NET 2.0+ method

                        }
                        else
                            oldstate = newstate;
                }
                else
                {
                    sb.AppendLine(line);
                }
            }
            if (sb.Length > 0)
            {
                CreateDictionary(oldstate, sb.ToString());
            }

        }

        public static string[] Chop(string data, int lineNums)
        {
            List<string> list = new List<string>();
            StringReader sr = new StringReader(data);
            string line = "";
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
                list.Add(sb.ToString());
            return list.ToArray();
        }

        //This is ugly
        public static void CreateDictionary(ParserState state, string data)
        {
            //browsers = new Dictionary<int,Browser>();
            //browserOss= new Dictionary<int,BrowserOS>();
            //browserRegs = new Dictionary<int,BrowserReg>();
            //browserTypes = new Dictionary<int,BrowserType>();
            //oss = new Dictionary<int,OS>();
            //osRegs = new Dictionary<int,OSReg>();
            //robots = new Dictionary<int,Robot>();

            UserAgentItem uai;
            string[] dataSpliced = null;

            switch (state)
            {
                case ParserState.Robot:
                    uai = new Robot();
                    robots.Clear();
                    break;
                case ParserState.Browser:
                    uai = new Browser();
                    browsers.Clear();
                    break;
                case ParserState.OS:
                    uai = new OS();
                    oss.Clear();
                    break;
                case ParserState.BrowserOS:
                    uai = new BrowserOS();
                    browserOss.Clear();
                    break;
                case ParserState.BrowserReg:
                    uai = new BrowserReg();
                    browserRegs.Clear();
                    break;
                case ParserState.BrowserType:
                    uai = new BrowserType();
                    browserTypes.Clear();
                    break;
                case ParserState.OSReg:
                    uai = new OSReg();
                    osRegs.Clear();
                    break;
                default:
                    return;
            }

            dataSpliced = Chop(data, uai.GetNumberItems());
            //Figure out what to create....... 

            foreach (string d in dataSpliced)
            {
                switch (state)
                {
                    case ParserState.Robot:
                        uai = new Robot();
                        uai.Intialize(d);
                        robots.Add(uai.ID, (Robot)uai);
                        break;
                    case ParserState.Browser:
                        uai = new Browser();
                        uai.Intialize(d);
                        browsers.Add(uai.ID, (Browser)uai);
                        break;
                    case ParserState.OS:
                        uai = new OS();
                        uai.Intialize(d);
                        oss.Add(uai.ID, (OS)uai);
                        break;
                    case ParserState.BrowserOS:
                        uai = new BrowserOS();
                        uai.Intialize(d);
                        browserOss.Add(uai.ID, (BrowserOS)uai);
                        break;
                    case ParserState.BrowserReg:
                        uai = new BrowserReg();
                        uai.Intialize(d);
                        browserRegs.Add(uai.ID, (BrowserReg)uai);
                        break;
                    case ParserState.BrowserType:
                        uai = new BrowserType();
                        uai.Intialize(d);
                        browserTypes.Add(uai.ID, (BrowserType)uai);
                        break;
                    case ParserState.OSReg:
                        uai = new OSReg();
                        uai.Intialize(d);
                        osRegs.Add(uai.ID, (OSReg)uai);
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
