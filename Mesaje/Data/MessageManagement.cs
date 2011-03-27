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
    [XmlRoot("Messages")]
    public class MessageManagement : List<Message>
    {
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
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MessageManagement));
                // Load the object saved above by using the Deserialize function
                return (MessageManagement)serializer.Deserialize(reader);
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

        public static MessageManagement UpdateXml()
        {
            // Create a new XmlDocument  
            XmlDocument doc = new XmlDocument();

            // Load data  
            doc.Load("http://mesaje.hruban.ro/listeazaXML.php");

            // Set up namespace manager for XPath  
            //XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            //ns.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // Get forecast with XPath  
            XmlNodeList nodes = doc.SelectNodes("/mesaje/mesaj");

            foreach (XmlNode node in nodes)
            {
                Console.WriteLine("{0}",
                                    node.FirstChild.InnerText);
            }

            return null;
        }

        /// <summary>
        /// Save the messages to a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        /// <param name="messages">The Messages to be saved.</param>
        public static void SaveToXml(string filePath, MessageManagement messages)
        {
            System.IO.FileStream stream = new System.IO.FileStream(filePath, FileMode.Create);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MessageManagement));
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

        [XmlIgnore()]
        public Message DisplayMessage
        {
            get
            {
                if (this.Count == 0)
                    return null;
                
                Random rand = new Random(DateTime.Now.Second);
                int displayMessagePos = rand.Next(1, this.Count);
                // TODO: make the function more random / use items that were not shown

                // TODO: make this a property / function / event
                Message returnMsg = this[displayMessagePos-1];
                returnMsg.Publish();

                return returnMsg;
            }
        }
    }
}
