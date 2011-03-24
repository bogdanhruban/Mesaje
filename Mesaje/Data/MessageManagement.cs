using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

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
        public static MessageManagement LoadXml(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;                
            }

            FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            XmlSerializer serializer = new XmlSerializer(typeof(MessageManagement));

            try
            {
                // Load the object saved above by using the Deserialize function
                MessageManagement loadedObj = (MessageManagement)serializer.Deserialize(reader);

                return loadedObj;
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Save the messages to a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        /// <param name="messages">The Messages to be saved.</param>
        public static void SaveToXml(string filePath, MessageManagement messages)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MessageManagement));
            System.IO.FileStream stream = new System.IO.FileStream(filePath, FileMode.Create);
            try
            {
                serializer.Serialize(stream, messages);
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        [XmlArray("Message")]
        public List<Message> Messages
        {
            get
            {
                return m_messages;
            }
        }

        public Message DisplayMessage
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
