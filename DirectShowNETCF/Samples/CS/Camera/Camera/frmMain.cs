using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DirectShowNETCF;
using System.Runtime.InteropServices;

namespace Camera
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            cam_ = new DirectShowNETCF.Camera.Camera();
            List<string> resolutions = cam_.getMediaTypes();
            cam_.CapType = DirectShowNETCF.Camera.CaptureType.PreviewStill;
            for (int i = 0; i < resolutions.Count; i++)
            {
                resolutionCombo.Items.Add(resolutions[i]);
            }
            capTypeCombo.SelectedIndex = 2;
            resolutionCombo.SelectedIndex = 0;
        }

        DirectShowNETCF.Camera.Camera cam_ = null;

        private void button1_Click(object sender, EventArgs e)
        {
            cam_.setMediaType(resolutionCombo.SelectedIndex);
            if (!cam_.init())
            {
                MessageBox.Show("Cannot Init camera");
            }
        }

        private void capTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cam_.CapType = (DirectShowNETCF.Camera.CaptureType)capTypeCombo.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!cam_.run(panel1.Handle))
            {
                MessageBox.Show("Cannot start camera");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cam_.setMediaType(resolutionCombo.SelectedIndex);
            cam_.init();
            cam_.run(panel1.Handle);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cam_.stillImage(textBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cam_.stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cam_.stop();
            cam_.release();
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            cam_.flashOn();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cam_.flashOff();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

    }
}