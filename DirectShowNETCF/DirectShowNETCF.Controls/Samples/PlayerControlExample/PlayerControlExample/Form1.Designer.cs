namespace PlayerControlExample
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.durationLabel = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.playerControl1 = new PlayerControl.PlayerControl();
            this.positionLabel = new System.Windows.Forms.Label();
            this.loopCB = new System.Windows.Forms.CheckBox();
            this.resolution = new System.Windows.Forms.Label();
            this.bitrate = new System.Windows.Forms.Label();
            this.volume = new System.Windows.Forms.Label();
            this.mute = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 18);
            this.button1.TabIndex = 1;
            this.button1.Text = "Play";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(82, 184);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 18);
            this.button2.TabIndex = 2;
            this.button2.Text = "Stop";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(161, 184);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 18);
            this.button3.TabIndex = 3;
            this.button3.Text = "Exit";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // durationLabel
            // 
            this.durationLabel.Location = new System.Drawing.Point(2, 256);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(233, 19);
            this.durationLabel.Text = "Duration:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(2, 203);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 17);
            this.button4.TabIndex = 5;
            this.button4.Text = "Seek";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // playerControl1
            // 
            this.playerControl1.Location = new System.Drawing.Point(3, 3);
            this.playerControl1.Name = "playerControl1";
            this.playerControl1.Size = new System.Drawing.Size(232, 180);
            this.playerControl1.TabIndex = 0;
            this.playerControl1.MediaEnded += new System.EventHandler(this.playerControl1_MediaEnded);
            this.playerControl1.MediaFailed += new System.EventHandler(this.playerControl1_MediaFailed);
            this.playerControl1.MediaProgress += new System.EventHandler<PlayerControl.ProgressEventArgs>(this.playerControl1_MediaProgress);
            // 
            // positionLabel
            // 
            this.positionLabel.Location = new System.Drawing.Point(2, 274);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(230, 19);
            this.positionLabel.Text = "Position:";
            // 
            // loopCB
            // 
            this.loopCB.Location = new System.Drawing.Point(120, 203);
            this.loopCB.Name = "loopCB";
            this.loopCB.Size = new System.Drawing.Size(59, 16);
            this.loopCB.TabIndex = 7;
            this.loopCB.Text = "Loop";
            this.loopCB.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // resolution
            // 
            this.resolution.Location = new System.Drawing.Point(3, 223);
            this.resolution.Name = "resolution";
            this.resolution.Size = new System.Drawing.Size(127, 19);
            this.resolution.Text = "Resolution:";
            // 
            // bitrate
            // 
            this.bitrate.Location = new System.Drawing.Point(131, 222);
            this.bitrate.Name = "bitrate";
            this.bitrate.Size = new System.Drawing.Size(105, 19);
            this.bitrate.Text = "B.Rate:";
            // 
            // volume
            // 
            this.volume.Location = new System.Drawing.Point(3, 239);
            this.volume.Name = "volume";
            this.volume.Size = new System.Drawing.Size(110, 19);
            this.volume.Text = "Volume:";
            // 
            // mute
            // 
            this.mute.Location = new System.Drawing.Point(176, 203);
            this.mute.Name = "mute";
            this.mute.Size = new System.Drawing.Size(59, 16);
            this.mute.TabIndex = 10;
            this.mute.Text = "Mute";
            this.mute.CheckStateChanged += new System.EventHandler(this.mute_CheckStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.mute);
            this.Controls.Add(this.volume);
            this.Controls.Add(this.bitrate);
            this.Controls.Add(this.resolution);
            this.Controls.Add(this.loopCB);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.durationLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.playerControl1);
            this.Name = "Form1";
            this.Text = "Player Control Example";
            this.ResumeLayout(false);

        }

        #endregion

        private PlayerControl.PlayerControl playerControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.CheckBox loopCB;
        private System.Windows.Forms.Label resolution;
        private System.Windows.Forms.Label bitrate;
        private System.Windows.Forms.Label volume;
        private System.Windows.Forms.CheckBox mute;
    }
}

