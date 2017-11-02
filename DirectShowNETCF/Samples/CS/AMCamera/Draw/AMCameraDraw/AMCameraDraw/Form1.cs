using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DirectShowNETCF.Enums;
using DirectShowNETCF.Structs;

namespace AMCameraDraw
{
    public partial class MainForm : Form
    {
        private DirectShowNETCF.Camera.AMCamera.AMCamera cam_;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cam_ = new DirectShowNETCF.Camera.AMCamera.AMCamera();
            DirectShowNETCF.Camera.AMCamera.AMResult res_ = cam_.init(false);
            if (res_ != DirectShowNETCF.Camera.AMCamera.AMResult.OK)
            {
                MessageBox.Show("Cannot init camera!\r\n Reason: " + res_.ToString());
                return;
            }

            if (!cam_.run(videoWindow.Handle))
            {
                MessageBox.Show("Cannot start camera!");
            }

            int width, height;
            RawFrameFormat format;
            cam_.getParams(out width, out height, out format);
            left.Maximum = width - 1;
            right.Maximum = width - 1;
            top.Maximum = height - 1;
            bottom.Maximum = height - 1;
            bottom.Value = height - 2;
            right.Value = width - 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam_.startDrawText(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cam_.stopDrawText();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Rect rect_;
                rect_.Left = (int)left.Value;
                rect_.Right = (int)right.Value;
                rect_.Top = (int)top.Value;
                rect_.Bottom = (int)bottom.Value;
                cam_.drawTarget(rect_, (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Rect rect_;
                rect_.Left = (int)left.Value;
                rect_.Right = (int)right.Value;
                rect_.Top = (int)top.Value;
                rect_.Bottom = (int)bottom.Value;
                cam_.drawTarget(rect_, (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cam_.release();
            Close();
        }

        private void top_ValueChanged(object sender, EventArgs e)
        {
            int type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE;
            if (radioButton2.Checked)
            {
                type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET;
            }

            Rect rect_;
            rect_.Left = (int)left.Value;
            rect_.Right = (int)right.Value;
            rect_.Top = (int)top.Value;
            rect_.Bottom = (int)bottom.Value;
            cam_.drawTarget(rect_, type_);
        }

        private void left_ValueChanged(object sender, EventArgs e)
        {
            int type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE;
            if (radioButton2.Checked)
            {
                type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET;
            }

            Rect rect_;
            rect_.Left = (int)left.Value;
            rect_.Right = (int)right.Value;
            rect_.Top = (int)top.Value;
            rect_.Bottom = (int)bottom.Value;
            cam_.drawTarget(rect_, type_);
        }

        private void right_ValueChanged(object sender, EventArgs e)
        {
            int type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE;
            if (radioButton2.Checked)
            {
                type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET;
            }

            Rect rect_;
            rect_.Left = (int)left.Value;
            rect_.Right = (int)right.Value;
            rect_.Top = (int)top.Value;
            rect_.Bottom = (int)bottom.Value;
            cam_.drawTarget(rect_, type_);
        }

        private void bottom_ValueChanged(object sender, EventArgs e)
        {
            int type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE;
            if (radioButton2.Checked)
            {
                type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET;
            }

            Rect rect_;
            rect_.Left = (int)left.Value;
            rect_.Right = (int)right.Value;
            rect_.Top = (int)top.Value;
            rect_.Bottom = (int)bottom.Value;
            cam_.drawTarget(rect_, type_);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.RECTANGLE;
            if (radioButton2.Checked)
            {
                type_ = (int)DirectShowNETCF.Camera.AMCamera.TargetType.TARGET;
            }

            Rect rect_;
            rect_.Left = (int)left.Value;
            rect_.Right = (int)right.Value;
            rect_.Top = (int)top.Value;
            rect_.Bottom = (int)bottom.Value;
            cam_.drawTarget(rect_, type_);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cam_.stopDrawTarget();
        }
    }
}