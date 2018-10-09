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
using System.Threading;

namespace FileManager
{
    public partial class Form1 : Form
    {
        static string buffer = "", bufferItem = "";
        FileInfo fileInfo = null;
        Form f = new Form();
        public Form1()
        {
            InitializeComponent();
            
            ToolStripMenuItem toolStripCopy = new ToolStripMenuItem("Copy");
            toolStripCopy.Click += ToolStripCopy_Click;
            ToolStripMenuItem toolStripPaste = new ToolStripMenuItem("Paste");
            toolStripPaste.Click += ToolStripPaste_Click;
            contextMenuStrip1.Items.AddRange(new[] { toolStripCopy, toolStripPaste });
        }

        private void ToolStripPaste_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(Psate));
            t.Start();
            
        }

        private void Psate()
        {
            fileInfo = new FileInfo(buffer);
            if (fileInfo.Exists)
            {

                //fileInfo.CopyTo(Path.Combine(textBox1.Text,bufferItem), true);
                try
                {
                    byte[] arr;
                    using (FileStream fstream = new FileStream(buffer, FileMode.Open))
                    {
                        arr = new byte[fstream.Length];
                        int interval = 0;
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Maximum = Convert.ToInt32(fstream.Length / 100);
                        }));
                        
                        for (int i = 0; i < fstream.Length / 100; i++)
                        {
                            fstream.Read(arr, interval, 100);
                            interval += 100;
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = i + 1;
                            }));
                            //Thread.Sleep(1000);
                        }
                    }
                    using (FileStream fstrealWrite = new FileStream(Path.Combine(textBox1.Text, bufferItem), FileMode.OpenOrCreate))
                    {
                        fstrealWrite.Write(arr, 0, arr.Length);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    MessageBox.Show("Copy is successful");
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                    }));
                    
                }
                OpenDirectory();
            }
        }

        private void ToolStripCopy_Click(object sender, EventArgs e)
        {
            buffer = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            bufferItem = listBox1.SelectedItem.ToString();
           

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
                listBox1.ContextMenuStrip = contextMenuStrip1;

            }
        }
    }
}
