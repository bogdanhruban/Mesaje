using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

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

            FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MessageManagement));
            
            try
            {
                // Load the object saved above by using the Deserialize function
                MessageManagement loadedObj = (MessageManagement)serializer.Deserialize(reader);
                this.m_messages = loadedObj.m_messages;
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
            }

            return false;
        }

        /// <summary>
        /// Save the messages to a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        void SaveToXml(string filePath)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MessageManagement));
            System.IO.FileStream stream = new System.IO.FileStream(filePath, FileMode.CreateNew);
            try
            {
                serializer.Serialize(stream, this);
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
