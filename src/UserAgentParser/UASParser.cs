using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using UserAgentStringLibrary.Tables;
using UserAgentStringLibrary.Util;

/*
 * Created by Adam Abonyi
 * 
 * Feel free to reuse this code in anyway. 
 * I had to use a piece of code under MS-PL licence so please respect it.
 * If you use it in any project i would be glad to hear about it (just curiouse where it is beeing used).
 * If you find any bugs (and there probably are some) or have any suggestions on how to complete or refine the code 
 * feel free to write me an email to adam.abonyi(at)gmail.com and i'll gladly add you to contributor list 
 * 
 * Contributors: 
 *  Anders Jonsson (www.mutlinet.se) - BugFixes and Refinement
 *  David Atherton (www.xunity.com) - BugFixes and Refinement
 *  David Mužátko (www.alza.cz) - Bug Report
*/
namespace UAParserSharp
{
    /// <summary>
    /// UAS Parser
    /// Class that comunicates with the server, downloads the information file
    /// and uses it to parse the UAS string
    /// </summary>
    public class UASParser
    {
        #region UASParser Parameters

        //User will be able to set this in config file, this will be default value
        private static string savePath = string.Empty;// + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UASParser");
        private static string localFileName = "data.dat";
        private static bool refresh = false; //D.A. define global refresh parameter to force reload of data
        private static string error = string.Empty; //D.A. Error message parameter
        private static ScheduleType schedule; //D.A. schedule enum parameter

        //More config file information
        private static string versionURL = @"http://user-agent-string.info/rpc/get_data.php?key=free&format=ini&ver=y";
        private static string iniFileURL = @"http://user-agent-string.info/rpc/get_data.php?key=free&format=ini";

        //Paths 
        public static readonly Uri UAImagesURL = new Uri(@"http://user-agent-string.info/pub/img/ua/");

        public static readonly Uri OSImagesURL = new Uri(@"http://user-agent-string.info/pub/img/os/");

        public static readonly Uri UserAgentStringURL = new Uri(@"http://user-agent-string.info/");

        private static DataTables dataTable;

        #endregion

        #region UASParser Enums

        //D.A. Enums created to allow scheduling of request to check for new version of the file
        public enum ScheduleType
        {
            None, Quarter_Hourly, Half_Hourly, Hourly, Half_Daily, Daily, Weekly, Monthly, Yearly
        }

        #endregion

        #region UASParser Properties

        public static string Data { get; set; }
        public static string DataVersion { get; set; }
        private static string localFilePath
        {
            get { return Path.Combine(savePath, localFileName); }
        }

        //D.A. Public Properties
        public string ErrorMsg { get { return error; } }  //D.A. Error message property

        #endregion

        #region UASParser Constructors (CTor)

        /// <summary>
        /// Constructor of the UAS Parser class.
        /// Checks if the directory exists and tries to create it.
        /// Checks for the data file with the information about User Agent Strings.
        /// If the file isn't found, it tries to download it. 
        /// The file is then loaded
        /// </summary>
        /// <param name="SavePath">String: Local path where the data.dat file will be saved and retrieved from</param>
        public UASParser(string SavePath)
        {
            savePath = SavePath;
            LoadParser();
        }

        /// <summary>
        /// D.A. Constructor of the UAS Parser class.
        /// </summary>
        /// <param name="SavePath">String: Local path where the data.dat file will be saved and retrieved from</param>
        /// <param name="Schedule">Enum: Specified schedule options for checking if a request should be made for version or data</param>
        /// <param name="Refresh">Boolean: Option to force a data refresh via webrequest.</param>
        public UASParser(string SavePath, ScheduleType Schedule, bool Refresh)
        {
            savePath = SavePath;
            schedule = Schedule;
            refresh = Refresh;
            LoadParser();
        }

        #endregion

        #region UASParser Support Functions

        /// <summary>
        /// D.A. Create new private function to allow for loading used by the CTors (Constructor)
        /// Basically allows you to check all items such as path and schedule before making the check/call.
        /// Checks if the directory exists and tries to create it.
        /// Checks for the data file with the information about User Agent Strings or if a Refresh is requested.
        /// If the file isn't found or a refresh is reqested, it tries to download it. 
        /// If file exists then loads data from file and checks if scheduled call should be made
        /// If request is to be made with an existing file, first it checks version by requests 
        /// if versions don't match then pull data but 
        /// if versions do match, reset date on file for schedule push forward.
        /// </summary>
        private void LoadParser()
        {
            error = string.Empty;
            try
            {
                //Check if directory for saving data exists
                if (!string.IsNullOrEmpty(savePath))
                {
                    DirectoryInfo di = new DirectoryInfo(savePath);
                    if (!di.Exists)
                    { di.Create(); }
                }

                //if we don't have any data loaded
                if (dataTable == null || refresh)
                {
                    dataTable = new DataTables();

                    //D.A. If the file does not exist then get data from URL
                    if (!File.Exists(localFilePath))
                    {
                        FetchDataFile();
                        //Load Data into Structures
                        if (Data == null) { error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "Could not load data. No internet connection or local copy of file", string.Empty); }
                        else { LoadData(Data); }
                    }
                    else
                    {
                        //D.A. should load from file, don't make new SendRequest
                        LoadDataFile();
                        //D.A. check if the schedule has been set and if it has, has the file expired based upon that schedule Or forcing refresh?
                        if (CheckScheduledFileExpire() || refresh)
                        {
                            //D.A. it would make sense if the file exists to check the version to see if needs updating nut refresh forces get anyway
                            string ver = GetNewestVersion();
                            if (IsOlderThen(ver) || refresh)
                            {
                                //D.A. data file needs updating based upon version or from forced refresh
                                GetLatestDataFile();
                            }
                            else
                            {
                                //D.A. does not need updating so change the datafiles datetime created to move schedule forward.
                                SetDataFileCreationTime();
                                //Load Data into Structures
                                if (Data == null) { error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "Could not load data. No local copy of file", string.Empty); }
                                else { LoadData(Data); }
                            }
                        }
                        else
                        {
                            //Load Data into Structures
                            if (Data == null) { error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "Could not load data. No internet connection or local copy of file", string.Empty); }
                            else { LoadData(Data); }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", e.Message, e.StackTrace);
            }
        }

        /// <summary>
        /// Creates the object representation of the DB from the 
        /// file into the DataTables object
        /// </summary>
        /// <param name="Data"></param>
        private void LoadData(string Data)
        {
            dataTable.LoadData(Data);
        }

        /// <summary>
        /// Parses the User-Agent-String 
        /// </summary>
        /// <param name="uas">User-Agent-String</param>
        /// <returns>UASrt - information about the UAS</returns>
        public UserAgentStringInfo Parse(string uas)
        {
            Robot robot = DataTables.Robots.Values.FirstOrDefault(x => x.UserAgentString == uas);

            if (robot != null)
            {
                var robotOs = robot.OsID.HasValue && DataTables.Oss.ContainsKey(robot.OsID.Value)
                             ? DataTables.Oss[robot.OsID.Value]
                             : null;
                return new UserAgentStringInfo(robot, robotOs);
            }

            var browserRes = DetectBrowser(uas);

            if (browserRes == null)
            {
                return new UserAgentStringInfo();
            }

            Browser browser = browserRes.Item1;
            int t = browser.TypeID;
            var browserType = DataTables.BrowserTypes[t].Type;

            OS os = null;
            if (DataTables.BrowserOss.ContainsKey(browser.ID))
            {
                os = DataTables.Oss[DataTables.BrowserOss[browser.ID].OSID];
            }
            else
            {
                foreach (var osr in DataTables.OSRegs.Values)
                {
                    PerlRegExpConverter prec = new PerlRegExpConverter(osr.RegString, null, Encoding.ASCII);
                    Regex r = prec.Regex;
                    if (r.IsMatch(uas))
                    {
                        os = DataTables.Oss[osr.OSID];
                        break;
                    }
                }
            }

            return new UserAgentStringInfo(browser, browserType, browserRes.Item2, os);
        }

        private static Tuple<Browser, string> DetectBrowser(string uas)
        {
            foreach (var br in DataTables.BrowserRegs.Values)
            {
                PerlRegExpConverter prec = new PerlRegExpConverter(br.RegString, null, Encoding.ASCII);
                Regex r = prec.Regex;
                Match m = r.Match(uas);
                if (m.Success)
                {
                    GroupCollection gc = m.Groups;

                    var browser = DataTables.Browsers[br.BrowserID];
                    foreach (Group g in gc)
                    {
                        double version;
                        if (double.TryParse(g.Value.Replace(".", string.Empty), out version))
                        {
                            var versionString = g.Value;
                            return new Tuple<Browser, string>(browser, versionString);
                        }
                    }

                    return new Tuple<Browser, string>(browser, null);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the latest datafile available from the website
        /// The url where the data file is downloaded from is located in the config file
        /// </summary>
        public void GetLatestDataFile()
        {
            string newversion = GetNewestVersion();
            if (IsOlderThen(newversion))
            {
                FetchDataFile();

                //Load into structures
                dataTable = new DataTables();
                LoadData(Data);
            }
        }

        /// <summary>
        /// Loads the Data File into the object
        /// Parses the Data File version
        /// </summary>
        /// <param name="data">DataFile in the form of string</param>
        private void LoadDataFile(string data)
        {
            if (DataCorrect(data))
            {
                Data = data;
                DataVersion = GetDataVersionString(data);
            }
        }

        /// <summary>
        /// Loads the Data File from its location on the local computer
        /// The file name and location can be set in the config file
        /// </summary>
        private void LoadDataFile()
        {
            string s;
            using (StreamReader sr = new StreamReader(localFilePath))
            {
                s = sr.ReadToEnd();
            }

            LoadDataFile(s);
        }

        /// <summary>
        /// Fetches the current data file from the UAS Parser website
        /// The url can be changed in the config file
        /// Checks at least for some consistency of the file
        /// </summary>
        private bool FetchDataFile()
        {
            string data = SendRequest(iniFileURL);
            LoadDataFile(data);

            try
            {
                if (DataCorrect(data))
                {
                    using (StreamWriter output = new StreamWriter(localFilePath))
                    {
                        output.Write(data);
                    }

                    return true;
                }

                //D.A. added this error check
                error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "Data validation failed.", string.Empty);
            }
            catch (Exception e)
            {
                error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", e.Message, e.StackTrace);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Parses the version string of the data file
        /// to get the Date when the data file was created
        /// </summary>
        /// <param name="version">Data file version string</param>
        /// <returns>DateTime of when the data file was created</returns>
        private DateTime ParseVersion(string version)
        {
            string date = version.Split('-')[0];
            return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
        }

        /// <summary>
        /// Compares the version strings of the currently loaded data file
        /// and the version string in the parameter 
        /// </summary>
        /// <param name="version">Version string of the data file</param>
        /// <returns>true if the current datafile is older then the version in the parameter</returns>
        private bool IsOlderThen(string version)
        {
            DateTime thisversion = ParseVersion(DataVersion);
            int thisnum = int.Parse(DataVersion.Split('-')[1]);
            DateTime paramversion = ParseVersion(version);
            int paramnum = int.Parse(version.Split('-')[1]);

            if (paramversion > thisversion || (paramversion == thisversion && paramnum > thisnum))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the data file version from the data file content int the parameter
        /// </summary>
        /// <param name="d">Data file content</param>
        /// <returns>Version string of the data file in the parameter</returns>
        private string GetDataVersionString(string d)
        {
            Regex re = new Regex("Version: [0-9]{8}-[0-9]{2}");
            Match m = re.Match(d.Substring(0, 150));
            if (m.Success)
            {
                return m.Value.Split(':')[1];
            }

            return null;
        }

        /// <summary>
        /// Checks if the data in the parameter is 
        /// </summary>
        /// <param name="d">Data file content</param>
        /// <returns>true if data seems ok</returns>
        private bool DataCorrect(string d)
        {
            return d.StartsWith("; Data (format ini) for UASparser");
        }

        /// <summary>
        /// Sends a request to the url specified in the parameter
        /// without any additional data
        /// </summary>
        /// <param name="surl">URL in string form</param>
        /// <returns>Response of the Webrequest</returns>
        private string SendRequest(string surl)
        {
            string s = string.Empty;
            using (WebClient wc = new WebClient())
            {
                s = wc.DownloadString(surl);
            }
            return s;
        }

        /// <summary>
        /// Gets the Newest version of the datafile on 
        /// the UAS Parser website
        /// </summary>
        /// <returns>Verison string</returns>
        public string GetNewestVersion()
        {
            return SendRequest(versionURL);
        }

        /// <summary>
        /// D.A. To save calls to the website for new data, need to have option to set schedule
        /// for call to get updated data. Saves subsequent calls from flooding website.
        /// </summary>
        /// <returns>Boolean - response for the current schedule based upon when the file was created. Checks if has "expired"</returns>
        private static bool CheckScheduledFileExpire()
        {
            error = string.Empty;
            bool getUpdatedData = false;
            try
            {
                // check if file exists
                if (File.Exists(localFilePath))
                {
                    // load file info
                    FileInfo dataFile = new FileInfo(localFilePath);

                    // get the tiem difference for use in calculating if call to get new data is required
                    TimeSpan ts = DateTime.Now.Subtract(dataFile.CreationTime);
                    double TotalMins = Math.Abs(ts.TotalMinutes);
                    double TotalDays = Math.Abs(ts.TotalDays);

                    switch (schedule)
                    {
                        case ScheduleType.Quarter_Hourly:
                            getUpdatedData = TotalMins >= 15;
                            break;
                        case ScheduleType.Half_Hourly:
                            getUpdatedData = TotalMins >= 30;
                            break;
                        case ScheduleType.Hourly:
                            getUpdatedData = TotalMins >= 60;
                            break;
                        case ScheduleType.Half_Daily:
                            getUpdatedData = TotalMins >= 720;
                            break;
                        case ScheduleType.Daily:
                            getUpdatedData = TotalDays >= 1;
                            break;
                        case ScheduleType.Weekly:
                            getUpdatedData = TotalDays >= 7;
                            break;
                        case ScheduleType.Monthly:
                            getUpdatedData = DateTime.Now.Year > dataFile.CreationTime.Year || DateTime.Now.Month > dataFile.CreationTime.Month;
                            break;
                        case ScheduleType.Yearly:
                            getUpdatedData = DateTime.Now.Year > dataFile.CreationTime.Year;
                            break;
                    }
                }
                else
                {
                    error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "File does not exist", string.Empty);
                }
            }
            catch (Exception e)
            {
                error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", e.Message, e.StackTrace);
                getUpdatedData = false;
            }
            return getUpdatedData;
        }

        /// <summary>
        /// D.A. Created to "reset" the creation date on the dat file so any subsequent checks will not make a web request until schedule met
        /// Effectively resets the schedule.
        /// </summary>
        private static void SetDataFileCreationTime()
        {
            error = string.Empty;
            try
            {
                //check if file exists
                if (File.Exists(localFilePath))
                {
                    //load file info
                    FileInfo dataFile = new FileInfo(localFilePath);
                    //set file creation date
                    dataFile.CreationTime = DateTime.Now;
                    dataFile.Refresh();
                }
                else
                {
                    error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", "File does not exist", string.Empty);
                }
            }
            catch (Exception e)
            {
                error += string.Format("Error: \r\n Message: {0}\r\n Stack Trace: {1}\r\n", e.Message, e.StackTrace);
            }
        }

        #endregion
    }
}