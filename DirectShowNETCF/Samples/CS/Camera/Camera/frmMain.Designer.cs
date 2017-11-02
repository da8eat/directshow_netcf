namespace Camera
{
    partial class frmMain
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
            this.capTypeCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.resolutionCombo = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(17, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 144);
            // 
            // capTypeCombo
            // 
            this.capTypeCombo.Items.Add("Preview");
            this.capTypeCombo.Items.Add("Still");
            this.capTypeCombo.Items.Add("PreviewStill");
            this.capTypeCombo.Location = new System.Drawing.Point(110, 144);
            this.capTypeCombo.Name = "capTypeCombo";
            this.capTypeCombo.Size = new System.Drawing.Size(101, 23);
            this.capTypeCombo.TabIndex = 1;
            this.capTypeCombo.SelectedIndexChanged += new System.EventHandler(this.capTypeCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 18);
            this.label1.Text = "Mode";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 18);
            this.label2.Text = "Resolution";
            // 
            // resolutionCombo
            // 
            this.resolutionCombo.Location = new System.Drawing.Point(110, 168);
            this.resolutionCombo.Name = "resolutionCombo";
            this.resolutionCombo.Size = new System.Drawing.Size(101, 23);
            this.resolutionCombo.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 20);
            this.button1.TabIndex = 6;
            this.button1.Text = "Init";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(61, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 20);
            this.button2.TabIndex = 7;
            this.button2.Text = "Start";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(115, 192);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 20);
            this.button3.TabIndex = 8;
            this.button3.Text = "Change Resolution";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 213);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(141, 23);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "\\Program Files\\test.jpg";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(159, 214);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 20);
            this.button4.TabIndex = 13;
            this.button4.Text = "Still";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(186, 240);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(48, 20);
            this.button5.TabIndex = 15;
            this.button5.Text = "exit";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(132, 240);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(48, 20);
            this.button6.TabIndex = 14;
            this.button6.Text = "Stop";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(63, 240);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(65, 20);
            this.button7.TabIndex = 20;
            this.button7.Text = "flash off";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(5, 240);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(57, 20);
            this.button8.TabIndex = 19;
            this.button8.Text = "flash on";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 263);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resolutionCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.capTypeCombo);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.Text = "Camera";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox capTypeCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox resolutionCombo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}

