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
using CustomUIControls;

namespace Mesaje
{
    public class MesajeApplication : Form
    {
        #region Tray items
        NotifyIcon notifyIcon;
        ContextMenu contextMenu;
        MenuItem exitMenuItem;
        MenuItem newMessageMenuItem;
        MenuItem optionsMenuItem;
        IContainer components;
        #endregion

        #region Locals
        MessageManagement messages;
        Timer messageDisplayTimer;
        TaskbarNotifier taskbarNotifier;
        #endregion

        public MesajeApplication()
        {
            notifyIcon = new NotifyIcon();
            contextMenu = new ContextMenu();
            exitMenuItem = new MenuItem();
            newMessageMenuItem = new MenuItem();
            optionsMenuItem = new MenuItem();

            // initialize the context menu
            contextMenu.MenuItems.AddRange(new MenuItem[] { exitMenuItem, optionsMenuItem, newMessageMenuItem });

            // initialize the menu items
            exitMenuItem.Index = 0;
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            optionsMenuItem.Index = 1;
            optionsMenuItem.Text = "&Options";
            optionsMenuItem.Click += new EventHandler(optionsMenuItem_Click);

            newMessageMenuItem.Index = 2;
            newMessageMenuItem.Text = "Arata mesaj &nou";
            newMessageMenuItem.Click += new EventHandler(newMessageMenuItem_Click);

            // create the notification icon
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resource.Application;

            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Text = "Mesaje diverse";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipText = "Apasati pe icoana pentru mesaje diverse.";
            notifyIcon.BalloonTipTitle = "Mesaje diverse";
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            // add the resize event
            this.Resize += new EventHandler(MesajeApplication_Resize);
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            AppSettingsReader appSettings = new AppSettingsReader();
            // set the timer
            messageDisplayTimer = new Timer();
            messageDisplayTimer.Interval = (int)(appSettings.GetValue("MessageInterval", typeof(int)));
            messageDisplayTimer.Tick += new EventHandler(MessageTimeout);
            messageDisplayTimer.Enabled = true;
            messageDisplayTimer.Start();

            taskbarNotifier = new TaskbarNotifier();
            taskbarNotifier.SetBackgroundBitmap(Resource.skin3, Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
            taskbarNotifier.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier.CloseClick += new EventHandler(CloseClick);

            LoadItems();

            // add a dummy item
            Mesaje.Data.Message dummy = new Data.Message();
            dummy.ID = 1;
            dummy.Body = "Dummy body";
            dummy.Title = "Dummy title -- Mooooou";
            messages.Add(dummy);
        }

        protected void LoadItems()
        {
            // read the config file
            try
            {
                // Get the appSettings section.
                AppSettingsReader appSettings = new AppSettingsReader();
                messages = MessageManagement.LoadXml((string)(appSettings.GetValue("MessagesXml", typeof(string))));
            }
            catch (Exception e)
            {
                Console.WriteLine("[LoadItems: {0}]", e.ToString());
            }
            finally
            {
                if (messages == null)
                    messages = new MessageManagement();
            }
        }

        protected override void Dispose(bool disposing)
        {
            AppSettingsReader appSettings = new AppSettingsReader();
            //MessageManagement.SaveToXml((string)(appSettings.GetValue("MessagesXml", typeof(string))), messages);

            notifyIcon.Visible = false;
            messageDisplayTimer.Stop();

            // Clean up any components being used.
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        private void MessageTimeout(object sender, EventArgs e)
        {
            // display a message window
            Data.Message msg = messages.DisplayMessage;
            
            taskbarNotifier.CloseClickable = true;
            taskbarNotifier.TitleClickable = true;
            taskbarNotifier.ContentClickable = true;
            taskbarNotifier.EnableSelectionRectangle = true;
            taskbarNotifier.KeepVisibleOnMousOver = true;	// Added Rev 002
            taskbarNotifier.ReShowOnMouseOver = false;			// Added Rev 002
            taskbarNotifier.Show(msg.Title, msg.Body, 500, 6000, 500);
        }

        #region TaskbarNotifierEvents
        private void TitleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Title clicked!");
        }
        private void ContentClick(object sender, EventArgs e)
        {
            MessageBox.Show("Content clicked!");
        }
        private void CloseClick(object sender, EventArgs e)
        {
            MessageBox.Show("Close clicked!");
            taskbarNotifier.Close();
        }
        #endregion


        private void MesajeApplication_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
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
