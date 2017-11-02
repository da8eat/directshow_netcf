using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AMCameraFocusZoom
{
    public partial class MainForm : Form
    {
        private DirectShowNETCF.Camera.AMCamera.AMCamera cam_ = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!cam_.focusPlus())
            {
                MessageBox.Show("cannot focus +");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!cam_.focusMinus())
            {
                MessageBox.Show("cannot focus -");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam_ = new DirectShowNETCF.Camera.AMCamera.AMCamera();
            DirectShowNETCF.Camera.AMCamera.AMResult res_ =
                cam_.init(false);

            if (res_ != DirectShowNETCF.Camera.AMCamera.AMResult.OK)
            {
                MessageBox.Show("Cannot init camera!\r\n Reason: " + res_.ToString());
                return;
            }

            if (!cam_.run(videoWindow.Handle))
            {
                MessageBox.Show("Cannot start camera!");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cam_.release();
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!cam_.autoFocusOn())
            {
                MessageBox.Show("Cannot turn on autofocus");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            cam_.autoFocusOff();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!cam_.zoomIn())
            {
                MessageBox.Show("cannot zoomIn");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!cam_.zoomOut())
            {
                MessageBox.Show("cannot zoomOut");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!cam_.flashOn())
            {
                MessageBox.Show("Cannot turn flash ON");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            cam_.flashOff();
        }
    }
}