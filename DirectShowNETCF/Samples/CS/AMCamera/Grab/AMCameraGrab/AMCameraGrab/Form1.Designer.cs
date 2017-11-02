namespace AMCameraGrab
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
            this.SuspendLayout();
            // 
            // videoWindow
            // 
            this.videoWindow.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoWindow.Location = new System.Drawing.Point(2, 2);
            this.videoWindow.Name = "videoWindow";
            this.videoWindow.Size = new System.Drawing.Size(235, 194);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "start";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 21);
            this.button2.TabIndex = 2;
            this.button2.Text = "exit";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(4, 224);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(230, 19);
            this.button3.TabIndex = 3;
            this.button3.Text = "Grab GrayScale";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(4, 249);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(230, 19);
            this.button4.TabIndex = 4;
            this.button4.Text = "Grab RGB565";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(4, 273);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(230, 19);
            this.button5.TabIndex = 5;
            this.button5.Text = "Grab Raw Buffer";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.videoWindow);
            this.Name = "MainForm";
            this.Text = "Grab";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel videoWindow;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

