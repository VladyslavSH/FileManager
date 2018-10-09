using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FileManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenDirectory()
        {
            listBox1.Items.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] directory = directoryInfo.GetDirectories();
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var item in directory)
            {
                listBox1.Items.Add(item);
            }
            foreach (var Ifile in files)
            {
                listBox1.Items.Add(Ifile);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenDirectory();
            
        }

        

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            
            if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
            {
                textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                listBox1.Items.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] directory = directoryInfo.GetDirectories();
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (var item in directory)
                {
                    listBox1.Items.Add(item);
                }
                foreach (var Ifile in files)
                {
                    listBox1.Items.Add(Ifile);
                }

            }
            else
            {
                Process.Start(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }
                else
                {
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }
                OpenDirectory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
            
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolStripMenuItem toolStripCopy = new ToolStripMenuItem("Copy");
                ToolStripMenuItem toolStripPaste = new ToolStripMenuItem("Paste");
                contextMenuStrip1.Items.AddRange(new[] { toolStripCopy, toolStripPaste });
                listBox1.ContextMenuStrip = contextMenuStrip1;
                
            }
        }
    }
}
