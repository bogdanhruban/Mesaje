using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mesaje.Data
{
    [XmlRoot("Message")]
    [XmlType("Message")]
    public class Message
    {
        uint id = 0;
        string body = null;
        string title = null;
        DateTime addDate;
        DateTime publishDate;
        DateTime lastPublished;
        uint timesPublished = 0;

        #region Properties

        [XmlAttribute("ID", typeof(uint))]
        public uint ID
        {
            get
            {
                return id;
            }
        }

        [XmlAttribute("Body", typeof(string))]
        public string Body
        {
            get
            {
                return body;
            }
        }

        [XmlAttribute("Title", typeof(string))]
        public string Title
        {
            get
            {
                return title;
            }
        }

        [XmlAttribute("AddDate", typeof(DateTime))]
        public DateTime AddDate
        {
            get
            {
                return addDate;
            }
        }

        [XmlAttribute("LastPublished", typeof(DateTime))]
        public DateTime LastPublished
        {
            get
            {
                return lastPublished;
            }
        }

        [XmlAttribute("PublishDate", typeof(DateTime))]
        public DateTime PublishDate
        {
            get
            {
                return publishDate;
            }
        }

        [XmlAttribute("TimesPublished", typeof(uint))]
        public uint TimesPublished
        {
            get
            {
                return timesPublished;
            }
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
