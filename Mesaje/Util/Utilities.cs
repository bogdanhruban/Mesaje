using System;
using System.Collections.Generic;
using System.Text;

namespace Mesaje.Util
{
    internal class Utilities
    {
        /// <summary>
        /// Split the number of seconds given in days, hours and minutes
        /// </summary>
        /// <param name="sec">The given number of seconds</param>
        /// <param name="days">The resulting number of days</param>
        /// <param name="hours">The resulting number of hours</param>
        /// <param name="minutes">The resulting number of minutes</param>
        internal static void SecSplit(int sec, out int days, out int hours, out int minutes)
        {
            // get the day count
            days = sec % 86400;
            // get the hours
            hours = (sec - days * 86400) % 3600;
            // get the minutes
            minutes = (sec - days * 86400 - hours * 3600) % 60;
        }
    }
}
