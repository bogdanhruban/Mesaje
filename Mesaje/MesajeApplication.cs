using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Configuration;
using Mesaje.Data;

namespace Mesaje
{
    public class MesajeApplication : Form
    {
        #region Tray items
        NotifyIcon m_notifyIcon;
        ContextMenu m_contextMenu;
        MenuItem m_exitMenuItem;
        MenuItem m_newMessageMenuItem;
        MenuItem m_optionsMenuItem;
        IContainer m_components;
        #endregion

        #region Locals
        MessageManagement m_messages;
        #endregion

        public MesajeApplication()
        {
            m_notifyIcon = new NotifyIcon();
            m_contextMenu = new ContextMenu();
            m_exitMenuItem = new MenuItem();
            m_newMessageMenuItem = new MenuItem();
            m_optionsMenuItem = new MenuItem();

            // initialize the context menu
            m_contextMenu.MenuItems.AddRange(new MenuItem[] { m_exitMenuItem, m_optionsMenuItem, m_newMessageMenuItem });

            // initialize the menu items
            m_exitMenuItem.Index = 0;
            m_exitMenuItem.Text = "E&xit";
            m_exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            m_optionsMenuItem.Index = 1;
            m_optionsMenuItem.Text = "&Options";
            m_optionsMenuItem.Click += new EventHandler(optionsMenuItem_Click);

            m_newMessageMenuItem.Index = 2;
            m_newMessageMenuItem.Text = "Arata mesaj &nou";
            m_newMessageMenuItem.Click += new EventHandler(newMessageMenuItem_Click);

            // create the notification icon
            m_notifyIcon = new NotifyIcon();
            m_notifyIcon.Icon = Resource.Application;

            m_notifyIcon.ContextMenu = m_contextMenu;
            m_notifyIcon.Text = "Mesaje diverse";
            m_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            m_notifyIcon.BalloonTipText = "Apasati pe icoana pentru mesaje diverse.";
            m_notifyIcon.BalloonTipTitle = "Mesaje diverse";
            m_notifyIcon.Visible = true;
            m_notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            // add the resize event
            this.Resize += new EventHandler(MesajeApplication_Resize);
            m_notifyIcon.Visible = true;
            m_notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            LoadItems();
        }

        protected void LoadItems()
        {
            // read the config file
            try
            {
                // Get the appSettings section.
                AppSettingsReader appSettings = new AppSettingsReader();
                m_messages = MessageManagement.LoadXml((string)(appSettings.GetValue("MessagesXml", typeof(string))));

            }
            catch (Exception e)
            {
                Console.WriteLine("[LoadItems: {0}]", e.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            AppSettingsReader appSettings = new AppSettingsReader();
            MessageManagement.SaveToXml((string)(appSettings.GetValue("MessagesXml", typeof(string))), m_messages);

            // Clean up any components being used.
            if (disposing)
                if (m_components != null)
                    m_components.Dispose();

            base.Dispose(disposing);
        }


        private void MesajeApplication_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                m_notifyIcon.Visible = true;
                m_notifyIcon.ShowBalloonTip(500);
                //this.Hide();
            }
            //else if (FormWindowState.Normal == this.WindowState)
            //{
            //    m_notifyIcon.Visible = false;
            //}
        }


        private void notifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void exitMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void optionsMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            MessageBox.Show("Options clicked!");
        }

        private void newMessageMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            MessageBox.Show("Show new message clicked!");
        }

        /// <summary>
        /// Entry point in the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MesajeApplication());
        }
    };

    
}
