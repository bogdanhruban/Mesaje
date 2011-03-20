using System;
using System.Collections.Generic;
using System.Text;

namespace Mesaje.Data
{
    public class Mesaj
    {
        public uint m_id = 0;
        public string m_body = null;
        public string m_title = null;
        public DateTime m_addDate;
        public DateTime m_publishDate;
        public DateTime m_lastPublished;
        public uint m_timesPublished = 0;
    }
}
