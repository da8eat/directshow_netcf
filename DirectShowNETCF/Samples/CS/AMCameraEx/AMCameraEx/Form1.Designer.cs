namespace AMCameraExExample
{
    partial class mainFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.effectNone = new System.Windows.Forms.RadioButton();
            this.effectGrayScale = new System.Windows.Forms.RadioButton();
            this.effectSepia = new System.Windows.Forms.RadioButton();
            this.effectBinarization = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.effectEdge = new System.Windows.Forms.RadioButton();
            this.cropOval = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 171);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(2, 174);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(76, 22);
            this.start.TabIndex = 1;
            this.start.Text = "start";
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(81, 174);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(76, 22);
            this.stop.TabIndex = 2;
            this.stop.Text = "stop";
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(159, 174);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(76, 22);
            this.exit.TabIndex = 3;
            this.exit.Text = "exit";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // effectNone
            // 
            this.effectNone.Checked = true;
            this.effectNone.Location = new System.Drawing.Point(3, 198);
            this.effectNone.Name = "effectNone";
            this.effectNone.Size = new System.Drawing.Size(100, 19);
            this.effectNone.TabIndex = 5;
            this.effectNone.Text = "None";
            this.effectNone.CheckedChanged += new System.EventHandler(this.effectNone_CheckedChanged);
            // 
            // effectGrayScale
            // 
            this.effectGrayScale.Location = new System.Drawing.Point(3, 217);
            this.effectGrayScale.Name = "effectGrayScale";
            this.effectGrayScale.Size = new System.Drawing.Size(100, 19);
            this.effectGrayScale.TabIndex = 6;
            this.effectGrayScale.TabStop = false;
            this.effectGrayScale.Text = "GrayScale";
            this.effectGrayScale.CheckedChanged += new System.EventHandler(this.effectGrayScale_CheckedChanged);
            // 
            // effectSepia
            // 
            this.effectSepia.Location = new System.Drawing.Point(3, 255);
            this.effectSepia.Name = "effectSepia";
            this.effectSepia.Size = new System.Drawing.Size(100, 19);
            this.effectSepia.TabIndex = 8;
            this.effectSepia.TabStop = false;
            this.effectSepia.Text = "Sepia";
            this.effectSepia.CheckedChanged += new System.EventHandler(this.effectSepia_CheckedChanged);
            // 
            // effectBinarization
            // 
            this.effectBinarization.Location = new System.Drawing.Point(3, 236);
            this.effectBinarization.Name = "effectBinarization";
            this.effectBinarization.Size = new System.Drawing.Size(100, 19);
            this.effectBinarization.TabIndex = 7;
            this.effectBinarization.TabStop = false;
            this.effectBinarization.Text = "Binarization";
            this.effectBinarization.CheckedChanged += new System.EventHandler(this.effectBinarization_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(109, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 22);
            this.button1.TabIndex = 9;
            this.button1.Text = "grab Frame";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // effectEdge
            // 
            this.effectEdge.Location = new System.Drawing.Point(3, 273);
            this.effectEdge.Name = "effectEdge";
            this.effectEdge.Size = new System.Drawing.Size(118, 19);
            this.effectEdge.TabIndex = 11;
            this.effectEdge.TabStop = false;
            this.effectEdge.Text = "Edge Detection";
            this.effectEdge.CheckedChanged += new System.EventHandler(this.effectEdge_CheckedChanged);
            // 
            // cropOval
            // 
            this.cropOval.Location = new System.Drawing.Point(117, 230);
            this.cropOval.Name = "cropOval";
            this.cropOval.Size = new System.Drawing.Size(118, 19);
            this.cropOval.TabIndex = 13;
            this.cropOval.TabStop = false;
            this.cropOval.Text = "Crop Oval";
            this.cropOval.CheckedChanged += new System.EventHandler(this.cropOval_CheckedChanged);
            // 
            // mainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.cropOval);
            this.Controls.Add(this.effectEdge);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.effectSepia);
            this.Controls.Add(this.effectBinarization);
            this.Controls.Add(this.effectGrayScale);
            this.Controls.Add(this.effectNone);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.panel1);
            this.Name = "mainFrm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.RadioButton effectNone;
        private System.Windows.Forms.RadioButton effectGrayScale;
        private System.Windows.Forms.RadioButton effectSepia;
        private System.Windows.Forms.RadioButton effectBinarization;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton effectEdge;
        private System.Windows.Forms.RadioButton cropOval;
    }
}

