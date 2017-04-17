using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        string Path3;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgOpenRom.ShowDialog() == DialogResult.OK)
            {
             Path3 = dlgOpenRom.FileName;
            }
        }

        private void closeROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Path3 = "";
            dlgOpenRom.FileName = "";
            dlgOpenRom.Reset();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((Path3 != null) && (Path3 != ""))
            {
                if (TextHandler.CheckROM(Path3))
                {
                    int u=TextHandler.Extract(Path3);
                    if (u == 0)
                        MessageBox.Show("Text Extracted.", "Success");
                    else MessageBox.Show("Unknown Error.", "Error");
                }
                else MessageBox.Show("Is this a Shantae ROM?", "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((Path3 != null) && (Path3 != ""))
            {
                if (TextHandler.CheckROM(Path3))
                {
                    int u=TextHandler.Inject(Path3);
                    if (u == 0) MessageBox.Show("Text Injected.", "Success");
                    else if (u == 0xFE) MessageBox.Show("Error in Text Format.", "Error");
                    else MessageBox.Show("Unknown Error.", "Error");
                }
                else MessageBox.Show("Is this a Shantae ROM?", "Error");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Made by Lorenzooone. 2017\nFor any question: https://twitter.com/Lorenzooone", "About");
        }
    }
}
