using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PlayerControlExample
{
    public partial class Form1 : Form
    {
        private int _volume = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                playerControl1.Loop = loopCB.Checked;
                playerControl1.Stop();
                playerControl1.OpenFile(ofd.FileName);
                resolution.Text = "Resolution: " + playerControl1.VideoWidth + "x" + playerControl1.VideoHeight;
                bitrate.Text = "B.Rate: " + playerControl1.BitRate.ToString();
                _volume = playerControl1.Volume;
                volume.Text = "Volume: " + playerControl1.Volume.ToString();
                durationLabel.Text = "Duration: " + playerControl1.GetDuration().ToString();
                playerControl1.Play();
            }
        }

        private void playerControl1_MediaFailed(object sender, EventArgs e)
        {
            MessageBox.Show("MediaFailed");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            playerControl1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void playerControl1_MediaEnded(object sender, EventArgs e)
        {
            MessageBox.Show("MediaEnded");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            playerControl1.Seek(new TimeSpan(0, 0, 0));
        }

        private void playerControl1_MediaProgress(object sender, PlayerControl.ProgressEventArgs e)
        {
            positionLabel.Text = "Position: " + e.Progress.ToString();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            playerControl1.Loop = loopCB.Checked;
        }

        private void mute_CheckStateChanged(object sender, EventArgs e)
        {
            if (mute.Checked)
            {
                playerControl1.Volume = 0;
            }
            else
            {
                playerControl1.Volume = _volume;
            }
        }

        private void grab_Click(object sender, EventArgs e)
        {

        }
    }
}