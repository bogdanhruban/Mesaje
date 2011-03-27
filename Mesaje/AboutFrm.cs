using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Mesaje
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();

            TxtDetails.Text = "";
            LblCopy.Text = "";
            LblRegistred.Text = "";

            // set the owner firm
            //LblRegistred.Text = Config.GetInstance().OwnerFirm.Name;

            System.Reflection.Assembly thisAssembly = this.GetType().Assembly;
            LblVersion.Text = thisAssembly.GetName().Version.ToString();

            // Get all Copyright attributes on this assembly
            object[] attributes = thisAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            // If there aren't any Copyright attributes, return an empty string
            if (attributes.Length > 0)
            {
                // If there is a Copyright attribute, return its value
                LblCopy.Text = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
            attributes = thisAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                // set the details
                TxtDetails.Text = ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}