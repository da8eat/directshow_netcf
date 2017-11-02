using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NullCamera
{
    public partial class Form1 : Form
    {
        DirectShowNETCF.NullCamera cam_ = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.Dispose();
                cam_ = null;
            }

            cam_ = new DirectShowNETCF.NullCamera();

            if (!cam_.init())
            {
                MessageBox.Show("Cannot Init camera");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.GotFrame -= OnFrame;
                cam_.Dispose();
            }

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                if (!cam_.run())
                {
                    MessageBox.Show("Cannot start camera");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.GotFrame += OnFrame;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.GotFrame -= OnFrame;
            }
        }

        private void ShowBitmap(Bitmap bmp)
        {
            pictureBox1.Image = bmp;
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
    }
}