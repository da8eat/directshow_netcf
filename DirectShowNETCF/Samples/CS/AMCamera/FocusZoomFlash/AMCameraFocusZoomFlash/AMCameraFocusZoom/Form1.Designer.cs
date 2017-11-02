namespace AMCameraFocusZoom
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.videoWindow = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // videoWindow
            // 
            this.videoWindow.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoWindow.Location = new System.Drawing.Point(1, 3);
            this.videoWindow.Name = "videoWindow";
            this.videoWindow.Size = new System.Drawing.Size(236, 203);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1, 208);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "start";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 208);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 20);
            this.button2.TabIndex = 2;
            this.button2.Text = "exit";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 255);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 18);
            this.button3.TabIndex = 3;
            this.button3.Text = "ZoomIn";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 274);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 18);
            this.button4.TabIndex = 4;
            this.button4.Text = "ZoomOut";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(160, 274);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 18);
            this.button5.TabIndex = 6;
            this.button5.Text = "Focus-";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(160, 255);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 18);
            this.button6.TabIndex = 5;
            this.button6.Text = "Focuc+";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(20, 232);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(89, 21);
            this.button7.TabIndex = 7;
            this.button7.Text = "AutoFocusOn";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(128, 232);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(89, 21);
            this.button8.TabIndex = 8;
            this.button8.Text = "AutoFocusOff";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(81, 274);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 18);
            this.button9.TabIndex = 11;
            this.button9.Text = "Flash Off";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(81, 255);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 18);
            this.button10.TabIndex = 10;
            this.button10.Text = "Flash On";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.videoWindow);
            this.Name = "MainForm";
            this.Text = "FocusZoomFlash";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel videoWindow;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}

