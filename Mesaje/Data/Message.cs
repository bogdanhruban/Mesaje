using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Mesaje.Data
{
    [XmlRoot("mesaj")]
    [XmlType("mesaj")]
    public class Message
    {
        uint id = 0;
        string body = null;
        string title = null;
        DateTime addDate = DateTime.MinValue;
        DateTime publishDate = DateTime.MinValue;
        DateTime lastPublished = DateTime.MinValue;
        uint timesPublished = 0;
        string author = null;
        string authorLink = null;

        #region Properties

        [XmlAttribute("id", typeof(uint))]
        public uint ID
        {
            get
            {
                return id;
            }
            internal set
            {
                id = value;
            }
        }

        [XmlAttribute("continut", typeof(string))]
        public string Body
        {
            get
            {
                return body;
            }
            internal set
            {
                body = value;
            }
        }

        [XmlAttribute("titlu", typeof(string))]
        public string Title
        {
            get
            {
                return title;
            }
            internal set
            {
                title = value;
            }
        }

        [XmlAttribute("dataadaugarii", typeof(DateTime))]
        public DateTime AddDate
        {
            get
            {
                return addDate;
            }
            internal set
            {
                addDate = value;
            }
        }

        [XmlAttribute("ultimapublicare", typeof(DateTime))]
        public DateTime LastPublished
        {
            get
            {
                return lastPublished;
            }
            internal set
            {
                lastPublished = value;
            }
        }

        [XmlAttribute("datapublicarii", typeof(DateTime))]
        public DateTime PublishDate
        {
            get
            {
                return publishDate;
            }
            internal set
            {
                publishDate = value;
            }
        }

        [XmlAttribute("numarpublicatii", typeof(uint))]
        public uint TimesPublished
        {
            get
            {
                return timesPublished;
            }
            internal set
            {
                timesPublished = value;
            }
        }

        [XmlAttribute("autor", typeof(uint))]
        public string Author
        {
            get
            {
                return author;
            }
            internal set
            {
                author = value;
            }
        }

        [XmlAttribute("autorlink", typeof(uint))]
        public string AuthorLink
        {
            get
            {
                return authorLink;
            }
            internal set
            {
                authorLink = value;
            }
        }

        #endregion

        #region Serialization
        /// <summary>
        /// Deserialize a Message from an XML node
        /// </summary>
        /// <param name="node">The XML structure containing the Message info.</param>
        /// <returns>A new Message with the information received from the XML</returns>
        public static Message FromXml(XmlNode node)
        {
            Message msg = new Message();

            // try to get all the available information from the node
            XmlNode tmpNode = null;
            
            // get the title
            tmpNode = node.SelectSingleNode("titlu");
            if (tmpNode != null)
            {
                msg.Title = tmpNode.InnerText;
            }

            // get the body
            tmpNode = node.SelectSingleNode("continut");
            if (tmpNode != null)
            {
                msg.Body = tmpNode.InnerText;
            }

            // get the author
            tmpNode = node.SelectSingleNode("autor");
            if (tmpNode != null)
            {
                msg.Author = tmpNode.InnerText;
            }

            // get the author link
            tmpNode = node.SelectSingleNode("autorlink");
            if (tmpNode != null)
            {
                msg.AuthorLink = tmpNode.InnerText;
            }

            // get the id
            tmpNode = node.SelectSingleNode("id");
            if (tmpNode != null)
            {
                msg.ID = uint.Parse(tmpNode.InnerText);
            }

            // get the author timesPublished
            tmpNode = node.SelectSingleNode("numarpublicatii");
            if (tmpNode != null)
            {
                msg.TimesPublished = uint.Parse(tmpNode.InnerText);
            }

            // get the author addDate
            tmpNode = node.SelectSingleNode("dataadaugarii");
            if (tmpNode != null)
            {
                msg.AddDate = DateTime.Parse(tmpNode.InnerText);
            }

            // get the author lastpublished
            tmpNode = node.SelectSingleNode("ultimapublicare");
            if (tmpNode != null)
            {
                msg.LastPublished = DateTime.Parse(tmpNode.InnerText);
            }

            // get the author datapublicarii
            tmpNode = node.SelectSingleNode("datapublicarii");
            if (tmpNode != null)
            {
                msg.PublishDate = DateTime.Parse(tmpNode.InnerText);
            }

            return msg;
        }

        /// <summary>
        /// Serialize into XML format the given Message object
        /// </summary>
        /// <param name="msg">The Message to be serialized</param>
        /// <returns>A XML string representation of the specifiedobject</returns>
        public static string ToXml(Message msg)
        {
            string xml = "<mesaj>";

            // add the Id
            xml += "<id>" + msg.ID + "</id>";

            // add the title
            if (msg.Title != null)
            {
                xml += "<titlu>" + msg.Title + "</titlu>";
            }

            // add the content
            if (msg.Body != null)
            {
                xml += "<continut>" + msg.Body + "</continut>";
            }

            // add the author
            if (msg.Author != null)
            {
                xml += "<autor>" + msg.Author + "</autor>";
            }

            // add the authorLink
            if (msg.AuthorLink != null)
            {
                xml += "<autorlink>" + msg.AuthorLink + "</autorlink>";
            }

            // add the timesPublished
            xml += "<numarpublicatii>" + msg.TimesPublished + "</numarpublicatii>";

            // add the addDate
            xml += "<dataadaugarii>" + msg.AddDate + "</dataadaugarii>";

            // add the lastpublished
            xml += "<ultimapublicare>" + msg.LastPublished + "</ultimapublicare>";

            // add the datapublicarii
            xml += "<datapublicarii>" + msg.PublishDate + "</datapublicarii>";

            xml += "</mesaj>";
            return xml;
        }

        /// <summary>
        /// Serialize into XML format the current Message object
        /// </summary>
        /// <returns>A XML string representation of the current object</returns>
        public string ToXml()
        {
            return Message.ToXml(this);
        }
        #endregion

        // TODO: change this into an event?
        public void Publish()
        {
            if (publishDate == DateTime.MinValue)
                publishDate = DateTime.Now;

            lastPublished = DateTime.Now;
            ++timesPublished;
        }
    }
}
