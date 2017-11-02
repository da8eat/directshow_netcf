namespace AMCameraDraw
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.left = new System.Windows.Forms.NumericUpDown();
            this.right = new System.Windows.Forms.NumericUpDown();
            this.top = new System.Windows.Forms.NumericUpDown();
            this.bottom = new System.Windows.Forms.NumericUpDown();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // videoWindow
            // 
            this.videoWindow.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoWindow.Location = new System.Drawing.Point(2, 2);
            this.videoWindow.Name = "videoWindow";
            this.videoWindow.Size = new System.Drawing.Size(235, 192);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 265);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "http://alexmogurenko.com";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(132, 258);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 18);
            this.button1.TabIndex = 2;
            this.button1.Text = "DrawTextOn";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(132, 276);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 18);
            this.button2.TabIndex = 3;
            this.button2.Text = "DrawTextOff";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 196);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(39, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "start";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(45, 196);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(34, 21);
            this.button4.TabIndex = 5;
            this.button4.Text = "exit";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.radioButton1.Location = new System.Drawing.Point(5, 223);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(74, 16);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.Text = "rectangle";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.radioButton2.Location = new System.Drawing.Point(5, 243);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(95, 16);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = false;
            this.radioButton2.Text = "semi-rectangle";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // left
            // 
            this.left.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.left.Location = new System.Drawing.Point(80, 216);
            this.left.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.left.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(59, 20);
            this.left.TabIndex = 8;
            this.left.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.left.ValueChanged += new System.EventHandler(this.left_ValueChanged);
            // 
            // right
            // 
            this.right.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.right.Location = new System.Drawing.Point(139, 216);
            this.right.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.right.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(59, 20);
            this.right.TabIndex = 9;
            this.right.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.right.ValueChanged += new System.EventHandler(this.right_ValueChanged);
            // 
            // top
            // 
            this.top.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.top.Location = new System.Drawing.Point(110, 195);
            this.top.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.top.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(59, 20);
            this.top.TabIndex = 10;
            this.top.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.top.ValueChanged += new System.EventHandler(this.top_ValueChanged);
            // 
            // bottom
            // 
            this.bottom.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.bottom.Location = new System.Drawing.Point(110, 236);
            this.bottom.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.bottom.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.bottom.Name = "bottom";
            this.bottom.Size = new System.Drawing.Size(59, 20);
            this.bottom.TabIndex = 11;
            this.bottom.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.bottom.ValueChanged += new System.EventHandler(this.bottom_ValueChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(171, 195);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(63, 18);
            this.button5.TabIndex = 12;
            this.button5.Text = "targetOn";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(171, 238);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(63, 18);
            this.button6.TabIndex = 13;
            this.button6.Text = "targetOff";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.bottom);
            this.Controls.Add(this.top);
            this.Controls.Add(this.right);
            this.Controls.Add(this.left);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.videoWindow);
            this.Name = "MainForm";
            this.Text = "Draw";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel videoWindow;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.NumericUpDown left;
        private System.Windows.Forms.NumericUpDown right;
        private System.Windows.Forms.NumericUpDown top;
        private System.Windows.Forms.NumericUpDown bottom;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}

