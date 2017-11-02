using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NullPlayer
{
    public partial class Form1 : Form
    {
        DirectShowNETCF.NullPlayer _player = new DirectShowNETCF.NullPlayer();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!_player.loadFile(ofd.FileName))
                {
                    MessageBox.Show("Cannot render file");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_player.start())
            {
                MessageBox.Show("Cannot play file");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _player.GotFrame -= OnFrame;
            _player.Dispose();
            Close();
        }

        private void OnFrame(object sender, DirectShowNETCF.FrameEventArgs e)
        {
            Bitmap bmp = new Bitmap(e.Width, e.Height);

            byte[] tmp = new byte[e.Height * e.Width * 2];
            Marshal.Copy(e.Frame, tmp, 0, tmp.Length);

            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(new Rectangle(0, 0, e.Width, e.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

            Marshal.Copy(tmp, 0, data.Scan0, tmp.Length);

            bmp.UnlockBits(data);
            this.Invoke(new Action<Bitmap>(ShowBitmap), bmp);
        }

        private void ShowBitmap(Bitmap bmp)
        {
            pictureBox1.Image = bmp;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _player.GotFrame += OnFrame;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _player.GotFrame -= OnFrame;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _player.stop();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _player.setVolume(0);
        }
    }
}