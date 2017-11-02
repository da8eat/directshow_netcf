using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DirectShowNETCF.Enums;
using DirectShowNETCF.Structs;

namespace AMCamera
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cam_ = new DirectShowNETCF.Camera.AMCamera.AMCamera();
        }

        DirectShowNETCF.Camera.AMCamera.AMCamera cam_ = null;

        private void Start_Click(object sender, EventArgs e)
        {
            if (cam_.init(true) != DirectShowNETCF.Camera.AMCamera.AMResult.OK)
            {
                MessageBox.Show("cannot init camera");
                return;
            }

            if (!cam_.run(panel1.Handle))
            {
                MessageBox.Show("cannot start camera");
                return;
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            cam_.stop();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            cam_.stop();
            cam_.release();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*long size = 0;
            IntPtr data = cam_.grabFrame(ref size);

            byte[] array = new byte[(int)size];
            System.Runtime.InteropServices.Marshal.Copy(data, array, 0, (int)size);
            DirectShowNETCF.PInvoke.LocalFree(data);

            System.IO.FileStream fs = new System.IO.FileStream(textBox1.Text, System.IO.FileMode.Create);
            fs.Write(array, 0, (int)size);
            fs.Flush();
            fs.Close();
            array = null;*/

            int width;
            int height;
            RawFrameFormat format;

            cam_.getParams(out width, out height, out format);

            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bool bFlag = cam_.getGrayScaleImage(data.Scan0);
            bmp.UnlockBits(data);
            if (bFlag)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            else
            {
                MessageBox.Show("Cannot grab frame");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int width;
            int height;
           RawFrameFormat format;

            cam_.getParams(out width, out height, out format);
            MessageBox.Show("Raw frame format:\r\n" +
                "Width: " + width + "\r\n" +
                "Height: " + height + "\r\n" +
                "Format: " + format.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cam_.startDrawText(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cam_.stopDrawText();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cam_.flashOn();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cam_.flashOff();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RawFrameFormat format_;
            int width, height;

            cam_.getParams(out width, out height, out format_);
            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

            bool bFlag = cam_.getRgb565(data.Scan0);
            bmp.UnlockBits(data);

            if (bFlag)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            else
            {
                MessageBox.Show("Cannot grab frame");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int width;
            int height;
            RawFrameFormat format;

            cam_.getParams(out width, out height, out format);

            Rect rect;
            rect.Left = 110;
            rect.Right = 150;
            rect.Top = 15;
            rect.Bottom = height - 12;

            cam_.drawTarget(rect, (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            cam_.stopDrawTarget();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!cam_.autoFocusOn())
            {
                MessageBox.Show("focus on");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (!cam_.autoFocusOff())
            {
                MessageBox.Show("focus off");
            }
        }
    }
}