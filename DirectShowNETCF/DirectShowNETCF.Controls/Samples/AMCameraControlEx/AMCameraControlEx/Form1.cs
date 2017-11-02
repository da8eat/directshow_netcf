using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AMCameraControlEx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            amCameraExControl1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = amCameraExControl1.GrabFrame();

            if (bmp != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                bmp.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            amCameraExControl1.eraseBitmap(1);
            amCameraExControl1.Overlay(Bitmaps.overay_image, 1, 0, 60);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //amCameraExControl1.OverlayTransparent(Bitmaps.error, 30, 100, Color.White);
            amCameraExControl1.eraseBitmap(2);
            amCameraExControl1.OverlayTransparent(Bitmaps.overay_image, 2, 0, 0, Color.White);
        }

        private void amCameraExControl1_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show("MouseDown" + e.Button.ToString() + e.X + " " + e.Y);
        }

        private void amCameraExControl1_MouseUp(object sender, MouseEventArgs e)
        {
            MessageBox.Show("MouseUp");
        }

        private void amCameraExControl1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("DoubleClick");
        }

        private void amCameraExControl1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            amCameraExControl1.eraseBitmap(1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            amCameraExControl1.eraseBitmap(2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            amCameraExControl1.BlendBitmap(Bitmaps.overay_image, 10, 10, 90, 125);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            amCameraExControl1.FixPreview(true);
        }
    }
}