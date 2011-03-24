using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Mesaje.Data
{
    public class MessageManagement
    {
        List<Message> m_messages = new List<Message>();

        /// <summary>
        /// Load the messages from a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        /// <returns>The true state of success for loading the XML data.</returns>
        bool LoadXml(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Save the messages to a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        void SaveToXml(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("File does not exist!");
            }
        }

        Message DisplayMessage
        {
            get
            {
                if (m_messages.Count == 0)
                    return null;
                
                Random rand = new Random(DateTime.Now.Second);
                int displayMessagePos = rand.Next(1, m_messages.Count);
                // TODO: make the function more random / use items that were not shown

                // TODO: make this a property / function / event
                Message returnMsg = m_messages[displayMessagePos];
                returnMsg.m_timesPublished++;
                returnMsg.m_lastPublished = DateTime.Now;

                return returnMsg;
            }
        }
    }
}
