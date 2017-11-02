using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;

namespace AMCameraGrab
{
    public partial class MainForm : Form
    {
        private DirectShowNETCF.Camera.AMCamera.AMCamera cam_;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam_ = new DirectShowNETCF.Camera.AMCamera.AMCamera();
            DirectShowNETCF.Camera.AMCamera.AMResult res_ = cam_.init(false);
            if (res_ != DirectShowNETCF.Camera.AMCamera.AMResult.OK)
            {
                MessageBox.Show("Cannot Init camera!\r\n Reason: " + res_);
                return;
            }

            if (!cam_.run(videoWindow.Handle))
            {
                MessageBox.Show("Cannot start camera!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int width, height;
            RawFrameFormat format;
            cam_.getParams(out width, out height, out format);

            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            bool bFlag = cam_.getGrayScaleImage(data.Scan0);
            bmp.UnlockBits(data);

            if (bFlag)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, ImageFormat.Bmp);
                }
            }
            else
            {
                MessageBox.Show("Cannot grab grayscale");
            }

            bmp.Dispose();
            bmp = null;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cam_.release();
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int width, height;
            RawFrameFormat format;
            cam_.getParams(out width, out height, out format);

            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format16bppRgb565);

            bool bFlag = cam_.getRgb565(data.Scan0);
            bmp.UnlockBits(data);

            if (bFlag)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, ImageFormat.Bmp);
                }
            }
            else
            {
                MessageBox.Show("Cannot grab RGB565");
            }

            bmp.Dispose();
            bmp = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            long size_ = 0;
            IntPtr rawBuffer = cam_.grabFrame(ref size_);

            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] buff = new byte[size_];
                System.Runtime.InteropServices.Marshal.Copy(rawBuffer, buff, 0, (int)size_);
                System.IO.FileStream fs = new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create);
                fs.Write(buff, 0, (int)size_);
                fs.Flush();
                fs.Close();
                fs = null;
            }

            PInvokes.LocalFree(rawBuffer);
        }
    }
}