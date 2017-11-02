using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Player
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            player = new DirectShowNETCF.Player.Player();
        }

        private DirectShowNETCF.Player.Player player = null;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                player.stop();
                if (!player.renderFile(ofd.FileName))
                    MessageBox.Show("render false");

                player.setVideoWindow(panel1.Handle);
                player.play();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            player.stop();
            player = null;
            Close();
        }
    }
}