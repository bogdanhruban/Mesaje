using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;

namespace Facturi.Util
{
    /// <summary>
    /// Error levels
    /// </summary>
    public enum LoggerErrorLevels : int
    {
        /// <summary>
        /// Information
        /// </summary>
        INFO = 0,
        /// <summary>
        /// Warning
        /// </summary>
        WARNING = 1,
        /// <summary>
        /// Error
        /// </summary>
        ERROR = 2,
        /// <summary>
        /// Fatal
        /// </summary>
        FATAL = 3,
        /// <summary>
        /// Debug
        /// </summary>
        DEBUG = 4
    }

    /// <summary>
    /// Log output path
    /// </summary>
    [Flags]
    public enum LoggerOutput { 
        /// <summary>
        /// Console
        /// </summary>
        CONSOLE,
        /// <summary>
        /// File
        /// </summary>
        FILE,
        /// <summary>
        /// File and Console
        /// </summary>
        FILECONSOLE = CONSOLE|FILE
    }

    /// <summary>
    /// Logger class
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Get the instance of the class
        /// </summary>
        /// <returns>Instance of the class</returns>
        public static Logger Instance 
        {
            get
            {

                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Logger() 
        {
            ReadConfiguration();
        }

        /// <summary>
        /// Property to get/set the output file name
        /// </summary>
        public string OutputFile
        {
            get { return m_outputFile; }
            set { m_outputFile = value; }
        }

        /// <summary>
        /// Property to get/set the output display
        /// </summary>
        public LoggerOutput LogOutput
        {
            get { return m_logOutput; }
            set { m_logOutput = value; }
        }

        /// <summary>
        /// Write an error message to the desired destination
        /// </summary>
        /// <param name="er">Exception to be written</param>
        /// <param name="erLevel">The error level of the message</param>
        public static void Write(Exception er, LoggerErrorLevels erLevel) {
            String msg = er.Message;
            try
            {
                if ((m_writeExtendedMessage) && (er.InnerException != null) && (er.InnerException.Message.Length > 0))
                {
                    msg += "\n" + er.InnerException.Message;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            Write(msg, erLevel);
        }

        /// <summary>
        /// Write an error message to the desired destination
        /// </summary>
        /// <param name="er">Exception to be written</param>
        /// <remarks>LoggerErrorLevel default value: ERROR</remarks>
        public static void Write(Exception er) {
            Write(er, LoggerErrorLevels.ERROR);
        }

        /// <summary>
        /// Write an error message to the desired destination
        /// </summary>
        /// <param name="msg">The message to be written</param>
        /// <param name="erLevel">The error level of the message</param>
        public static void Write(string msg, LoggerErrorLevels erLevel) 
        {
            if ((!m_showDebugInfo) && (erLevel == LoggerErrorLevels.DEBUG))
                return;
            string outMsg = DateTime.Now.ToShortDateString() + " "+DateTime.Now.ToLongTimeString();
            switch (erLevel)
            {
                case LoggerErrorLevels.INFO:
                    {
                        outMsg += " INFO: ";
                        break;
                    }
                case LoggerErrorLevels.WARNING:
                    {
                        outMsg += " WARNING: ";
                        break;
                    }
                case LoggerErrorLevels.ERROR:
                    {
                        outMsg += " ERROR: ";
                        break;
                    }
                case LoggerErrorLevels.FATAL:
                    {
                        outMsg += " FATAL: ";
                        break;
                    }
                case LoggerErrorLevels.DEBUG:
                    {
                        outMsg += " DEBUG: ";
                        break;
                    }
            }

            string fullInfo = "";
            if (erLevel != LoggerErrorLevels.INFO){
                StackTrace st = new StackTrace(true);
                int frameIndex = 0;

                // skip frames not from fqnOfCallingClass
                while (frameIndex < st.FrameCount)
                {
                    StackFrame frame = st.GetFrame(frameIndex);
                    if (frame != null && !frame.GetMethod().DeclaringType.Name.Equals("Logger"))
                    {
                        break;
                    }
                    ++frameIndex;
                }
                //++frameIndex;
                while (frameIndex < st.FrameCount)
                {
                    // now frameIndex is the first 'user' caller frame
                    StackFrame locationFrame = st.GetFrame(frameIndex);
                    string methodName = "", className = "", fileName = "", lineNumber = "";

                    if (locationFrame != null)
                    {
                        System.Reflection.MethodBase method = locationFrame.GetMethod();

                        if (method != null)
                        {
                            methodName = method.Name;
                            if (method.DeclaringType != null)
                            {
                                className = method.DeclaringType.FullName;
                            }
                        }
                        fileName = System.IO.Path.GetFileName(locationFrame.GetFileName());
                        lineNumber = locationFrame.GetFileLineNumber().ToString(System.Globalization.NumberFormatInfo.InvariantInfo);

                        // Combine all location info
                        fullInfo += className + '.' + methodName + '(' + fileName + ':' + lineNumber + "):\n";
                    }
                    if (methodName.ToUpper().Equals("MAIN"))
                        break;
                    ++frameIndex;       
                }
                //outMsg += fullInfo + "::";
            }
            outMsg += msg+"\n"+fullInfo;
            if ((m_logOutput & LoggerOutput.CONSOLE )== LoggerOutput.CONSOLE)
            {
                //display to the console
                Console.WriteLine(outMsg);
            }
            if ((m_logOutput & LoggerOutput.FILE) == LoggerOutput.FILE)
            {
                //write to file (append)
                System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(m_outputFile,(!m_overriteLogFlag));
                fileWriter.WriteLine(outMsg);
                fileWriter.Flush();
                fileWriter.Close();
            }
        }

        /// <summary>
        /// Write an error message to the desired destination
        /// </summary>
        /// <param name="msg">The message to be written</param>
        /// <remarks>LoggerErrorLevel default value: INFO></remarks>
        public static void Write(string msg) {
            Write(msg, LoggerErrorLevels.INFO);
        }

        /// <summary>
        /// Read the config file and ajust the output accordingly
        /// </summary>
        private void ReadConfiguration(){
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(m_configFile);
                try
                {
                    XmlNode outFileNode = doc.FirstChild.SelectSingleNode("Logger/OutputFile");
                    if (outFileNode != null)
                    {
                        m_outputFile = outFileNode.InnerText;
                        m_outputFile.Replace("%APP_PATH%", Application.StartupPath);
                        //m_overriteLogFlag = outFileNode.Attributes["overrite"].Value.Equals("0") ? false : true;
                    }
                
                }
                catch (Exception ex_outFile) {
                    Console.WriteLine(ex_outFile.Message);
                }
                try
                {
                    XmlNode outputDirNode = doc.FirstChild.SelectSingleNode("Logger/OutputDirection");
                    if (outputDirNode != null)
                    {
                        switch (outputDirNode.InnerText.ToUpper()) { 
                            case "FILE":
                                m_logOutput = LoggerOutput.FILE;
                                break;
                            case "CONSOLE":
                                m_logOutput = LoggerOutput.CONSOLE;
                                break;
                            case "FILECONSOLE":
                                m_logOutput = LoggerOutput.FILECONSOLE;
                                break;
                            default:
                                m_logOutput = LoggerOutput.FILE;
                                break;
                        }
                    }
                }
                catch (Exception ex_logOut) {
                    Console.WriteLine(ex_logOut.Message);
                }
                try {
                    XmlNode extendErrMsg = doc.FirstChild.SelectSingleNode("Logger/WriteExtendedErrorMessages");
                    if (extendErrMsg != null)
                    {
                        //m_writeExtendedMessage = bool.Parse(extendErrMsg.InnerText);
                        bool.TryParse(extendErrMsg.InnerText, out m_writeExtendedMessage);
                    }
                }
                catch (Exception ex_extendErr) {
                    m_writeExtendedMessage = true;
                    Console.WriteLine(ex_extendErr.Message);
                }

                try
                {
                    XmlNode debugNode = doc.FirstChild.SelectSingleNode("Logger/Debug");
                    if (debugNode != null)
                    {
                        //m_writeExtendedMessage = bool.Parse(extendErrMsg.InnerText);
                        bool.TryParse(debugNode.Attributes["active"].Value, out m_showDebugInfo);
                    }
                }
                catch (Exception ex_debug)
                {
                    m_showDebugInfo = false;
                    Console.WriteLine(ex_debug.Message);
                }

            }
            catch  {
                Console.WriteLine("Config file does not exist.");
            }
        }

        //private LoggerErrorLevels logLevels = LoggerErrorLevels.INFO;
        private static LoggerOutput m_logOutput = LoggerOutput.FILE;
        private static string m_outputFile = Application.StartupPath+@"\log.txt";
        private static Logger instance = null;
        private static bool m_overriteLogFlag = false;
        private static bool m_writeExtendedMessage = true;
        private static bool m_showDebugInfo = false;
        private static string m_configFile = @"/XML/Application/Config.xml";
    }   
}
