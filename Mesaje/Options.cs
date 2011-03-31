using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mesaje.Util;

namespace Mesaje
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            // set the background transparent for the skin immages
            Bitmap bm1 = new Bitmap(Resource.skin);
            bm1.MakeTransparent(Color.FromArgb(255, 0, 255));
            pictureBox1.Image = bm1;

            Bitmap bm2 = new Bitmap(Resource.skin2);
            bm2.MakeTransparent(Color.FromArgb(255, 0, 255));
            pictureBox2.Image = bm2;

            Bitmap bm3 = new Bitmap(Resource.skin3);
            bm3.MakeTransparent(Color.FromArgb(255, 0, 255));
            pictureBox3.Image = bm3;

            // initialize pannels
            panelMessagesGeneral.Top = panelGeneral.Top;
            panelMessagesGeneral.Left = panelGeneral.Left;

            panelMessagesUpdate.Top = panelGeneral.Top;
            panelMessagesUpdate.Left = panelGeneral.Left;

            panelSkinGeneral.Top = panelGeneral.Top;
            panelSkinGeneral.Left = panelGeneral.Left;

            treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "general":
                    panelGeneral.Visible = true;
                    panelMessagesGeneral.Visible = false;
                    panelMessagesUpdate.Visible = false;
                    panelSkinGeneral.Visible = false;
                    break;
                case "message":
                case "msgGeneral":
                    panelGeneral.Visible = false;
                    panelMessagesGeneral.Visible = true;
                    panelMessagesUpdate.Visible = false;
                    panelSkinGeneral.Visible = false;
                    break;
                case "msgUpdate":
                    panelGeneral.Visible = false;
                    panelMessagesGeneral.Visible = false;
                    panelMessagesUpdate.Visible = true;
                    panelSkinGeneral.Visible = false;
                    break;
                case "skin":
                    panelGeneral.Visible = false;
                    panelMessagesGeneral.Visible = false;
                    panelMessagesUpdate.Visible = false;
                    panelSkinGeneral.Visible = true;
                    break;
                default:
                    Logger.Write("Undefined node selected", LoggerErrorLevels.ERROR);
                    break;
            }
        }
    }
}
