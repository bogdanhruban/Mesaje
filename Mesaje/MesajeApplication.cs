﻿using System;
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
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem exitMenuButtom;
        private ToolStripMenuItem ajutorToolStripMenuItem;
        private ToolStripMenuItem despreToolStripMenuItem;
        TaskbarNotifier taskbarNotifier;
        #endregion

        public MesajeApplication()
        {
            InitializeComponent();

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
            MessageManagement.UpdateXml();

            // add a dummy item
            Mesaje.Data.Message dummy = new Data.Message();
            dummy.ID = 1;
            dummy.Body = "Dummy body,the red cow is crossing the street.";
            dummy.Title = "Dummy title";
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

            if (taskbarNotifier == null)
            {
                taskbarNotifier = new TaskbarNotifier();
                taskbarNotifier.SetBackgroundBitmap(Resource.skin3, Color.FromArgb(255, 0, 255));
                taskbarNotifier.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
                taskbarNotifier.TitleRectangle = new Rectangle(150, 57, 125, 28);
                taskbarNotifier.ContentRectangle = new Rectangle(75, 92, 215, 55);
                taskbarNotifier.TitleClick += new EventHandler(TitleClick);
                taskbarNotifier.ContentClick += new EventHandler(ContentClick);
                taskbarNotifier.CloseClick += new EventHandler(CloseClick);
            }
            
            taskbarNotifier.CloseClickable = true;
            taskbarNotifier.TitleClickable = true;
            taskbarNotifier.ContentClickable = true;
            taskbarNotifier.EnableSelectionRectangle = true;
            taskbarNotifier.KeepVisibleOnMousOver = true;	// Added Rev 002
            taskbarNotifier.ReShowOnMouseOver = true;			// Added Rev 002

            messageDisplayTimer.Stop();
            taskbarNotifier.Show(msg.Title, msg.Body, 500, 1000, 500);
            messageDisplayTimer.Start();

            // original values
            //textBoxDelayShowing.Text = "500";
            //textBoxDelayStaying.Text = "3000";
            //textBoxDelayHiding.Text = "500";
            //checkBoxSelectionRectangle.Checked = true;
            //checkBoxTitleClickable.Checked = false;
            //checkBoxContentClickable.Checked = true;
            //checkBoxCloseClickable.Checked = true;
            //checkBoxKeepVisibleOnMouseOver.Checked = true;		// Added Rev 002
            //checkBoxReShowOnMouseOver.Checked = false;			// Added Rev 002
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
            taskbarNotifier = null;
        }
        #endregion


        private void MesajeApplication_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Hide();
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
            {
                //this.WindowState = FormWindowState.Normal;
                this.Show();
            }

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
            
            messageDisplayTimer.Stop();
            MessageTimeout(null, null);
            messageDisplayTimer.Start();
        }

        private void about_Click(object Sender, EventArgs e)
        {
            FrmAbout about = new FrmAbout();
            about.ShowDialog();
        }

        /// <summary>
        /// Entry point in the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MesajeApplication());
        }

        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajutorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.despreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuButtom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 348);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(479, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.ajutorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(479, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuButtom});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.testToolStripMenuItem.Text = "Fisier";
            // 
            // ajutorToolStripMenuItem
            // 
            this.ajutorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.despreToolStripMenuItem});
            this.ajutorToolStripMenuItem.Name = "ajutorToolStripMenuItem";
            this.ajutorToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.ajutorToolStripMenuItem.Text = "Ajutor";
            // 
            // despreToolStripMenuItem
            // 
            this.despreToolStripMenuItem.Name = "despreToolStripMenuItem";
            this.despreToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.despreToolStripMenuItem.Text = "Despre";
            this.despreToolStripMenuItem.Click += new EventHandler(about_Click);
            // 
            // exitMenuButtom
            // 
            this.exitMenuButtom.Name = "exitMenuButtom";
            this.exitMenuButtom.Size = new System.Drawing.Size(152, 22);
            this.exitMenuButtom.Text = "Iesire";
            this.exitMenuButtom.Click += new EventHandler(exitMenuItem_Click);
            // 
            // MesajeApplication
            // 
            this.ClientSize = new System.Drawing.Size(479, 370);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MesajeApplication";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    };

    
}
