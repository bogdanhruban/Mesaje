using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Mesaje.Data
{
    public class MessageManagement
    {
        // TODO: replace with Message objects
        List<string> m_messages = new List<string>();

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
    }
}
