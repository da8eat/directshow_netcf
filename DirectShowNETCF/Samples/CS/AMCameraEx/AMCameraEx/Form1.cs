using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using ;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace AMCameraExExample
{
    public partial class mainFrm : Form
    {
       // private Camera.AMCameraEx.AMCameraEx cam_ = null;

        public mainFrm()
        {
            InitializeComponent();
        }
        Camera.AMCameraEx.AMCameraEx cam_ = null;

        private void start_Click(object sender, EventArgs e)
        {
            try
            {
                if (cam_ != null)
                {
                    cam_.release();
                    cam_ = null;
                }

                cam_ = new Camera.AMCameraEx.AMCameraEx();

                List<string> types = cam_.getMediaTypes();
                if (types.Count > 0)
                {
                    cam_.setMediaType(0);
                }

                Camera.AMCameraEx.AMResultEx result = cam_.init(Camera.AMCameraEx.RotationType.Auto);

                if (result != Camera.AMCameraEx.AMResultEx.OK)
                {
                    MessageBox.Show("Cannot init camera!\r\n Result: " + result);
                    return;
                }

                if (!cam_.run(panel1.Handle))
                {
                    MessageBox.Show("Cannot run :(");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.release();
                cam_ = null;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (cam_ != null)
            {
                cam_.release();
                cam_ = null;
            }

            Close();
        }

        private void switchEffect()
        {
            if (cam_ != null)
            {
                if (effectNone.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.None);
                }
                else if (effectGrayScale.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.GrayScale);
                }
                else if (effectBinarization.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.Binarization);
                }
                else if (effectSepia.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.Sepia);
                }
                else if (effectEdge.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.EdgeDetection);
                }
                else if (cropOval.Checked)
                {
                    cam_.applyEffect(Camera.AMCameraEx.Effets.CropOval);
                }
            }
        }

        private void effectNone_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();    
        }

        private void effectGrayScale_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();
        }

        private void effectBinarization_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();
        }

        private void effectSepia_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr frame = cam_.grabFrame();
            int width, height;
            cam_.getRect(out width, out height);

            byte[] buff = new byte[3 * width * height];
            Marshal.Copy(frame, buff, 0, buff.Length);

            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            Marshal.Copy(buff, 0, data.Scan0, buff.Length);

            bmp.UnlockBits(data);


            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(sfd.FileName, ImageFormat.Bmp);
            }

            bmp.Dispose();
            bmp = null;
            /*
             * do something with buffer
             * RGB24 buffer
             */

            //PInvokes.LocalFree(frame);
        }

        private void effectEdge_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();
        }

        private void cropOval_CheckedChanged(object sender, EventArgs e)
        {
            switchEffect();
        }
    }
}