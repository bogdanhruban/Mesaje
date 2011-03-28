using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesaje
{
    internal class Config
    {
        int _timeoutDisplayNotificationWindow = 3000; // 6 seconds
        int _publishInterval = 10000; // 1 hour
        int _messageUpdateInterval = 6000; // 1 hour
        bool _randomWindows = false;    // use all the bnotification windows available or just the selected one?
        int _notificationWindowId = 1;
        static Config _instance = new Config();

        /// <summary>
        /// Get the instance of the Config object.
        /// </summary>
        internal static Config Instance
        {
            get
            {
                return _instance;
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
