using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Mesaje.Util;

namespace Mesaje
{
    internal class Config
    {
        #region Variables
        static Config _instance;

        // app update
        bool _appUpdateAtStartup = false;
        bool _appUpdateNever = false;
        int _appUpdateInterval = 3600000;

        // message update
        bool _messageUpdateAtStartup = false;
        bool _messageUpdateNever = false;
        int _messageUpdateInterval = 3600000; // 1 hour

        // message categories
        List<String> _messageCategories = new List<string>();
        bool _allMessageCategories = true;

        // skin chooser
        bool _randomSkins = true;    // use all the notification windows available or just the selected one?
        int _skinId = 1;

        int _timeoutDisplayNotificationWindow = 6000; // 6 seconds
        int _publishInterval = 3600000; // 1 hour

        bool _startWithWindows = false;
        #endregion

        #region Read / Write config
        /// <summary>
        /// Read the configuration options from file
        /// </summary>
        /// <returns>A new Config object</returns>
        static Config ReadConfigFile()
        {
            Config cfg = new Config();

            AppSettingsReader appSettings = new AppSettingsReader();
            try
            {
                cfg._appUpdateAtStartup = bool.Parse((string)appSettings.GetValue("AppUpdateAtStartup", typeof(string)));
                cfg._appUpdateNever = bool.Parse((string)appSettings.GetValue("AppUpdateNeverUpdate", typeof(string)));
                cfg._appUpdateInterval = int.Parse((string)appSettings.GetValue("AppUpdateInterval", typeof(string)));

                cfg._messageUpdateAtStartup = bool.Parse((string)appSettings.GetValue("MessageUpdateAtStartup", typeof(string)));
                cfg._messageUpdateNever = bool.Parse((string)appSettings.GetValue("MessageUpdateNeverUpdate", typeof(string)));
                cfg._messageUpdateInterval = int.Parse((string)appSettings.GetValue("MessageUpdateInterval", typeof(string)));

                cfg._allMessageCategories = bool.Parse((string)appSettings.GetValue("UseAllCategories", typeof(string)));

                cfg._randomSkins = bool.Parse((string)appSettings.GetValue("UseRandomSkins", typeof(string)));
                cfg._skinId = int.Parse((string)appSettings.GetValue("SkinId", typeof(string)));

                cfg._timeoutDisplayNotificationWindow = int.Parse((string)appSettings.GetValue("TimeoutDisplayNotificationWindow", typeof(string)));
                cfg._publishInterval = int.Parse((string)appSettings.GetValue("PublishInterval", typeof(string)));

                cfg._startWithWindows = bool.Parse((string)appSettings.GetValue("StartWithWindows", typeof(string)));

            }
            catch (Exception e)
            {
                Logger.Write(e, LoggerErrorLevels.ERROR);
            }
            return cfg;
        }
        #endregion

        #region Application Update
        /// <summary>
        /// Get / Set the application update at startup
        /// </summary>
        internal bool AppUpdateAtStartup
        {
            get
            {
                return _appUpdateAtStartup;
            }
            set
            {
                _appUpdateAtStartup = value;
            }
        }

        /// <summary>
        /// Get / Set the application update - never update
        /// </summary>
        internal bool AppUpdateNeverUpdate
        {
            get
            {
                return _appUpdateNever;
            }
            set
            {
                _appUpdateNever = value;
            }
        }

        /// <summary>
        /// Get / Set the application update interval
        /// </summary>
        internal int AppUpdateInterval
        {
            get
            {
                return _appUpdateInterval;
            }
            set
            {
                _appUpdateInterval = value;
            }
        }
        #endregion

        #region Message Update
        /// <summary>
        /// Get / Set the message update at startup
        /// </summary>
        internal bool MessageUpdateAtStartup
        {
            get
            {
                return _messageUpdateAtStartup;
            }
            set
            {
                _messageUpdateAtStartup = value;
            }
        }

        /// <summary>
        /// Get / Set the message update - never update
        /// </summary>
        internal bool MessageUpdateNeverUpdate
        {
            get
            {
                return _messageUpdateNever;
            }
            set
            {
                _messageUpdateNever = value;
            }
        }

        /// <summary>
        /// Get / Set the message update interval
        /// </summary>
        internal int MessageUpdateInterval
        {
            get
            {
                return _messageUpdateInterval;
            }
            set
            {
                _messageUpdateInterval = value;
            }
        }
        #endregion

        #region Message Categories
        /// <summary>
        /// Get access to the message categories
        /// </summary>
        internal List<string> MessageCategories
        {
            get
            {
                return _messageCategories;
            }
        }

        /// <summary>
        /// Use all the message categories
        /// </summary>
        internal bool UseAllCategories
        {
            get
            {
                return _allMessageCategories;
            }
            set
            {
                _allMessageCategories = value;
            }
        }
        #endregion

        #region Skin chooser
        /// <summary>
        /// Use random skins for the notification window
        /// </summary>
        internal bool UseRandomSkins
        {
            get
            {
                return _randomSkins;
            }
            set
            {
                _randomSkins = value;
            }
        }

        /// <summary>
        /// Get the used skin id
        /// </summary>
        internal int SkinId
        {
            get
            {
                return _skinId;
            }
            set
            {
                _skinId = value;
            }
        }
        #endregion

        #region Notification Window
        /// <summary>
        /// Get / Set the timeout for the NotificationWindow
        /// </summary>
        internal int TimeoutDisplayNotificationWindow
        {
            get
            {
                return _timeoutDisplayNotificationWindow;
            }
            set
            {
                _timeoutDisplayNotificationWindow = value;
            }
        }

        /// <summary>
        /// Get / Set the interval after a new message is displayed
        /// </summary>
        internal int PublishInterval
        {
            get
            {
                return _publishInterval;
            }
            set
            {
                _publishInterval = value;
            }
        }
        #endregion

        /// <summary>
        /// Get the instance of the Config object.
        /// </summary>
        internal static Config Instance
        {
            get
            {
                if (_instance == null)
                    _instance = ReadConfigFile();

                return _instance;
            }
        }

        /// <summary>
        /// Is the application starting with Windows?
        /// </summary>
        internal bool StartWithWindows
        {
            get
            {
                return _startWithWindows;
            }
            set
            {
                _startWithWindows = value;
            }
        }
    }
}
