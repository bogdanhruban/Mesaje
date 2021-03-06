﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Mesaje.Util;

namespace Mesaje.Data
{
    [XmlRoot("mesaje")]
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

            MessageManagement msgM = new MessageManagement();
            try
            {
                // Create a new XmlDocument  
                XmlDocument doc = new XmlDocument();

                // Load data  
                doc.Load(filePath);

                XmlNodeList nodes = doc.SelectNodes("/mesaje/mesaj");

                foreach (XmlNode node in nodes)
                {
                    msgM.Add(Message.FromXml(node));
                }
            }
            catch (Exception e)
            {
                Logger.Write(e, LoggerErrorLevels.ERROR);
            }

            return msgM;
        }

        public static MessageManagement UpdateXml()
        {
            MessageManagement msgM = new MessageManagement();
            try
            {
                // Create a new XmlDocument  
                XmlDocument doc = new XmlDocument();

                // Load data  
                doc.Load("http://mesaje.hruban.ro/listeazaXML.php");

                XmlNodeList nodes = doc.SelectNodes("/mesaje/mesaj");

                foreach (XmlNode node in nodes)
                {
                    msgM.Add(Message.FromXml(node));
                }
            }
            catch (Exception e)
            {
                Logger.Write(e, LoggerErrorLevels.ERROR);
            }

            return msgM;
        }

        /// <summary>
        /// Save the messages to a XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        /// <param name="messages">The Messages to be saved.</param>
        public static void SaveToXml(string filePath, MessageManagement messages)
        {
            System.IO.FileStream stream = new System.IO.FileStream(filePath, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(stream);
            try
            {
                sWriter.WriteLine("<mesaje>");
                foreach (Message msg in messages)
                {
                    sWriter.WriteLine(msg.ToXml());
                }
                sWriter.WriteLine("</mesaje>");
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                
                sWriter.Flush();
                sWriter.Close();
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
