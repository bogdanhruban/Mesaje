using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
