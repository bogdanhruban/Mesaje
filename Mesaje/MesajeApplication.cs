using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Configuration;
using Mesaje.Data;
using CustomUIControls;
using Mesaje.Util;

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
        #endregion

        #region Locals
        MessageManagement messages = new MessageManagement();
        System.Object lockMessages = new System.Object();

        Timer messageDisplayTimer;
        Timer messageUpdateTimer;
        
        TaskbarNotifier taskbarNotifier1;
        TaskbarNotifier taskbarNotifier2;
        TaskbarNotifier taskbarNotifier3;

        Options optDialog = new Options();

        BackgroundWorker backWorker = new BackgroundWorker();
        #endregion

        #region Update MessageList
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
            }
            else
            {
                worker.ReportProgress(10);
                MessageManagement newList = MessageManagement.UpdateXml();
                worker.ReportProgress(60);
                // merge lists / replace old list?
                // replace first -- investigate later :)
                if (newList != null)
                {
                    lock (lockMessages)
                    {
                        messages = newList;
                    }
                }
                worker.ReportProgress(100);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if ((e.Cancelled == true))
            //{
            //    this.lblStatusBarInfo.Text = "Canceled!";
            //}

            if (!(e.Error == null))
            {
                //this.lblStatusBarInfo.Text = ("Error: " + e.Error.Message);
                Logger.Write(e.Error.Message, LoggerErrorLevels.ERROR);
            }

            //else
            //{
            //    //this.lblStatusBarInfo.Text = "Done!";
            //    // TODO: move this on a Timer ... with late removal
            //    lblStatusBarInfo.Visible = false;
            //    progressStatusBar.Visible = false;
            //}
        }

        //private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    lblStatusBarInfo.Visible = true;
        //    progressStatusBar.Visible = true;

        //    this.lblStatusBarInfo.Text = "Updating message list";
        //    this.progressStatusBar.Value = e.ProgressPercentage;
        //}
        #endregion

        public MesajeApplication()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon();
            contextMenu = new ContextMenu();
            exitMenuItem = new MenuItem();
            newMessageMenuItem = new MenuItem();
            optionsMenuItem = new MenuItem();

            // set up the Message Updater
            backWorker.WorkerReportsProgress = true;
            backWorker.WorkerSupportsCancellation = true;
            backWorker.DoWork += new DoWorkEventHandler(bw_DoWork);
            //backWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // initialize the context menu
            contextMenu.MenuItems.AddRange(new MenuItem[] { exitMenuItem, optionsMenuItem, newMessageMenuItem });

            // initialize the menu items
            exitMenuItem.Index = 2;
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            optionsMenuItem.Index = 1;
            optionsMenuItem.Text = "&Options";
            optionsMenuItem.Click += new EventHandler(optionsMenuItem_Click);

            newMessageMenuItem.Index = 0;
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
            //messageDisplayTimer.Interval = (int)(appSettings.GetValue("MessageInterval", typeof(int)));
            messageDisplayTimer.Interval = Config.Instance.PublishInterval;
            messageDisplayTimer.Tick += new EventHandler(MessageTimeout);
            messageDisplayTimer.Enabled = true;
            messageDisplayTimer.Start();


            messageUpdateTimer = new Timer();
            messageUpdateTimer.Interval = Config.Instance.MessageUpdateInterval;
            messageUpdateTimer.Tick += new EventHandler(UpdateMessagesTimeout);
            messageUpdateTimer.Enabled = true;
            messageUpdateTimer.Start();

            // initialize the Notification windows
            taskbarNotifier1 = new TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(Resource.skin, Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
            taskbarNotifier1.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier1.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier1.CloseClick += new EventHandler(CloseClick);

            taskbarNotifier2 = new TaskbarNotifier();
            taskbarNotifier2.SetBackgroundBitmap(Resource.skin2, Color.FromArgb(255, 0, 255));
            taskbarNotifier2.SetCloseBitmap(Resource.close2, Color.FromArgb(255, 0, 255), new Point(280, 57));
            taskbarNotifier2.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier2.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier2.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier2.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier2.CloseClick += new EventHandler(CloseClick);

            taskbarNotifier3 = new TaskbarNotifier();
            taskbarNotifier3.SetBackgroundBitmap(Resource.skin3, Color.FromArgb(255, 0, 255));
            taskbarNotifier3.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
            taskbarNotifier3.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier3.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier3.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier3.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier3.CloseClick += new EventHandler(CloseClick);

            //LoadItems();
            //MessageManagement.UpdateXml();
            // start updating the message list
            UpdateMessagesTimeout(null, null);

            // add a dummy item
            Mesaje.Data.Message dummy = new Data.Message();
            dummy.ID = 1;
            dummy.Body = "Dummy body,the red cow is crossing the street.";
            dummy.Title = "Dummy title";
            lock (lockMessages)
            {
                messages.Add(dummy);
            }

            this.WindowState = FormWindowState.Minimized;
        }

        protected void LoadItems()
        {
            // read the config file
            try
            {
                // Get the appSettings section.
                AppSettingsReader appSettings = new AppSettingsReader();
                lock (lockMessages)
                {
                    messages = MessageManagement.LoadXml((string)(appSettings.GetValue("MessagesXml", typeof(string))));
                }
            }
            catch (Exception e)
            {
                Logger.Write(e, LoggerErrorLevels.ERROR);
            }
            finally
            {
                lock (lockMessages)
                {
                    if (messages == null)
                        messages = new MessageManagement();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            AppSettingsReader appSettings = new AppSettingsReader();
            //MessageManagement.SaveToXml((string)(appSettings.GetValue("MessagesXml", typeof(string))), messages);

            notifyIcon.Visible = false;
            messageDisplayTimer.Stop();
            
            if (backWorker.WorkerSupportsCancellation == true)
            {
                backWorker.CancelAsync();
            }

            // Clean up any components being used.
            //if (disposing)
            //    if (components != null)
            //        components.Dispose();

            base.Dispose(disposing);
        }
        
        private void UpdateMessagesTimeout(object sender, EventArgs e)
        {
            if (!backWorker.IsBusy)
            {
                backWorker.RunWorkerAsync();
            }
        }

        private void DisplayNotificationWindow(string title, string body, int timeout, int winId)
        {
            TaskbarNotifier notifyWind = null;
            switch (winId)
            {
                case 1:
                    notifyWind = taskbarNotifier1;
                    break;
                case 2:
                    notifyWind = taskbarNotifier2;
                    break;
                case 3:
                default:
                    notifyWind = taskbarNotifier3;
                    break;
            }

            if (notifyWind == null)
            {
                // recreate the window
                notifyWind = CreateNotificationWindow(winId);
            }

            notifyWind.Show(title, body, 500, timeout, 500);
        }

        private TaskbarNotifier CreateNotificationWindow(int winId)
        {
            TaskbarNotifier notify = new TaskbarNotifier();

            switch (winId)
            {
                case 1:
                    // skin 1
                    notify.SetBackgroundBitmap(Resource.skin, Color.FromArgb(255, 0, 255));
                    notify.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
                    notify.TitleRectangle = new Rectangle(150, 57, 125, 28);
                    notify.ContentRectangle = new Rectangle(75, 92, 215, 55);
                    break;
                case 2:
                    //skin 2
                    notify.SetBackgroundBitmap(Resource.skin2, Color.FromArgb(255, 0, 255));
                    notify.SetCloseBitmap(Resource.close2, Color.FromArgb(255, 0, 255), new Point(280, 57));
                    notify.TitleRectangle = new Rectangle(150, 57, 125, 28);
                    notify.ContentRectangle = new Rectangle(75, 92, 215, 55);
                    break;
                case 3:
                default:
                    //skin 3
                    notify.SetBackgroundBitmap(Resource.skin3, Color.FromArgb(255, 0, 255));
                    notify.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
                    notify.TitleRectangle = new Rectangle(150, 57, 125, 28);
                    notify.ContentRectangle = new Rectangle(75, 92, 215, 55);
                    break;
            }

            notify.CloseClickable = true;
            notify.TitleClickable = false;
            notify.ContentClickable = false;
            notify.EnableSelectionRectangle = true;
            notify.KeepVisibleOnMousOver = true;	// Added Rev 002
            notify.ReShowOnMouseOver = true;			// Added Rev 002

            return notify;
        }

        private void MessageTimeout(object sender, EventArgs e)
        {
            Data.Message msg = null;
            // display a message window
            lock (lockMessages)
            {
                msg = messages.DisplayMessage;
            }

            if (msg == null)
            {
                Logger.Write("No DisplayMessage was received.", LoggerErrorLevels.WARNING);
                return;
            }

            //if (taskbarNotifier3 == null)
            //{
            //    taskbarNotifier3 = new TaskbarNotifier();
            //    taskbarNotifier3.SetBackgroundBitmap(Resource.skin3, Color.FromArgb(255, 0, 255));
            //    taskbarNotifier3.SetCloseBitmap(Resource.close, Color.FromArgb(255, 0, 255), new Point(280, 57));
            //    taskbarNotifier3.TitleRectangle = new Rectangle(150, 57, 125, 28);
            //    taskbarNotifier3.ContentRectangle = new Rectangle(75, 92, 215, 55);
            //    taskbarNotifier3.TitleClick += new EventHandler(TitleClick);
            //    taskbarNotifier3.ContentClick += new EventHandler(ContentClick);
            //    taskbarNotifier3.CloseClick += new EventHandler(CloseClick);
            //}
            
            //taskbarNotifier3.CloseClickable = true;
            //taskbarNotifier3.TitleClickable = false;
            //taskbarNotifier3.ContentClickable = false;
            //taskbarNotifier3.EnableSelectionRectangle = true;
            //taskbarNotifier3.KeepVisibleOnMousOver = true;	// Added Rev 002
            //taskbarNotifier3.ReShowOnMouseOver = true;			// Added Rev 002

            //taskbarNotifier3.Show(msg.Title, msg.Body, 500, Config.Instance.TimeoutDisplayNotificationWindow, 500);

            DisplayNotificationWindow(msg.Title, msg.Body, Config.Instance.TimeoutDisplayNotificationWindow, 3);
        }

        #region TaskbarNotifierEvents
        private void TitleClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Title clicked!");
        }
        private void ContentClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Content clicked!");
        }
        private void CloseClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Close clicked!");
            taskbarNotifier3.Close();
            taskbarNotifier3 = null;
        }
        #endregion


        private void MesajeApplication_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Visible = false;
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
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    //this.WindowState = FormWindowState.Normal;
            //    this.Show();
            //}

            //// Activate the form.
            //this.Activate();
            //optDialog.ShowDialog();
        }

        private void exitMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void optionsMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            //MessageBox.Show("Options clicked!");
            optDialog.ShowDialog();
        }

        private void newMessageMenuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            //MessageBox.Show("Show new message clicked!");
            MessageTimeout(null, null);
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
            this.SuspendLayout();
            // 
            // MesajeApplication
            // 
            this.ClientSize = new System.Drawing.Size(479, 370);
            this.MaximizeBox = false;
            this.Name = "MesajeApplication";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    };

    
}
