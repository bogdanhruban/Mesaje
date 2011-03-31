using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Mesaje.Util;

namespace Mesaje
{
    internal class Config
    {
        int _timeoutDisplayNotificationWindow = 6000; // 6 seconds
        int _publishInterval = 3600000; // 1 hour
        int _messageUpdateInterval = 3600000; // 1 hour
        bool _randomWindows = false;    // use all the notification windows available or just the selected one?
        int _notificationWindowId = 1;
        static Config _instance;

        bool _startWithWindows = false;

        // app update
        bool _appUpdateAtStartup = false;
        bool _appUpdateNever = false;
        int _appUpdateInterval = 3600000;


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
                cfg._timeoutDisplayNotificationWindow = int.Parse((string)appSettings.GetValue("TimeoutDisplayNotificationWindow", typeof(string)));
                cfg._publishInterval = int.Parse((string)appSettings.GetValue("PublishInterval", typeof(string)));
                cfg._messageUpdateInterval = int.Parse((string)appSettings.GetValue("MessageUpdateInterval", typeof(string)));
                cfg._randomWindows = bool.Parse((string)appSettings.GetValue("UseRandomWindows", typeof(string)));
                cfg._notificationWindowId = int.Parse((string)appSettings.GetValue("NotificationWindowId", typeof(string)));
            }
            catch (Exception e)
            {
                Logger.Write(e, LoggerErrorLevels.ERROR);
            }
            return cfg;
        }

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

        /// <summary>
        /// Get / Set the interval when updates for the message list are checked
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

        /// <summary>
        /// Get / Set the notification window to be used
        /// </summary>
        internal int NotificationWindowId
        {
            get
            {
                return _notificationWindowId;
            }
            set
            {
                _notificationWindowId = value;
            }
        }

        /// <summary>
        /// Get / Set if all the notification windows should be used
        /// </summary>
        internal bool UseRandomWindows
        {
            get
            {
                return _randomWindows;
            }
            set
            {
                _randomWindows = value;
            }
        }
    }
}
