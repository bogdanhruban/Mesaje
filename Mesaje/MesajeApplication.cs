using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Mesaje
{
    public class MesajeApplication : Form
    {
        #region Tray items
        NotifyIcon m_notifyIcon;
        ContextMenu m_contextMenu;
        MenuItem m_exitMenuItem;
        IContainer m_components;
        #endregion

        public MesajeApplication()
        {
            m_notifyIcon = new NotifyIcon();
            m_contextMenu = new ContextMenu();
            m_exitMenuItem = new MenuItem();

            // initialize the context menu
            m_contextMenu.MenuItems.AddRange(new MenuItem[] { m_exitMenuItem });

            // initialize the menu items
            m_exitMenuItem.Index = 0;
            m_exitMenuItem.Text = "E&xit";
            m_exitMenuItem.Click += new EventHandler(m_exitMenuItem_Click);

            // create the notification icon
            m_notifyIcon = new NotifyIcon();
            // TODO: set the icon
            //m_notifyIcon.Icon = new Icon("application.ico");

            m_notifyIcon.ContextMenu = m_contextMenu;
            m_notifyIcon.Text = "Mesaje diverse";
            m_notifyIcon.Visible = true;
            m_notifyIcon.DoubleClick += new EventHandler(m_notifyIcon_DoubleClick);
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
                if (m_components != null)
                    m_components.Dispose();

            base.Dispose(disposing);
        }

        private void m_notifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void m_exitMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MesajeApplication());
        }
    };

    
}
