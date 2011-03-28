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
        DateTime addDate = DateTime.Now;
        DateTime publishDate;
        DateTime lastPublished;
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
            tmpNode = node.SelectSingleNode("/mesaj/titlu");
            if (tmpNode != null)
            {
                msg.Title = tmpNode.InnerText;
            }

            // get the body
            tmpNode = node.SelectSingleNode("/mesaj/continut");
            if (tmpNode != null)
            {
                msg.Body = tmpNode.InnerText;
            }

            // get the author
            tmpNode = node.SelectSingleNode("/mesaj/autor");
            if (tmpNode != null)
            {
                msg.Author = tmpNode.InnerText;
            }

            // get the author link
            tmpNode = node.SelectSingleNode("/mesaj/autorlink");
            if (tmpNode != null)
            {
                msg.AuthorLink = tmpNode.InnerText;
            }

            // get the author id
            tmpNode = node.SelectSingleNode("/mesaj/id");
            if (tmpNode != null)
            {
                msg.ID = uint.Parse(tmpNode.InnerText);
            }

            // get the author id
            tmpNode = node.SelectSingleNode("/mesaj/id");
            if (tmpNode != null)
            {
                msg.ID = uint.Parse(tmpNode.InnerText);
            }

            // get the author timesPublished
            tmpNode = node.SelectSingleNode("/mesaj/numarpublicatii");
            if (tmpNode != null)
            {
                msg.TimesPublished = uint.Parse(tmpNode.InnerText);
            }

            // get the author addDate
            tmpNode = node.SelectSingleNode("/mesaj/dataadaugarii");
            if (tmpNode != null)
            {
                msg.AddDate = DateTime.Parse(tmpNode.InnerText);
            }

            // get the author lastpublished
            tmpNode = node.SelectSingleNode("/mesaj/ultimapublicare");
            if (tmpNode != null)
            {
                msg.LastPublished = DateTime.Parse(tmpNode.InnerText);
            }

            // get the author datapublicarii
            tmpNode = node.SelectSingleNode("/mesaj/datapublicarii");
            if (tmpNode != null)
            {
                msg.PublishDate = DateTime.Parse(tmpNode.InnerText);
            }

            return msg;
        }

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
