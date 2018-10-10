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
        DirectoryInfo directoryInfo = null;
        public Form1()
        {
            InitializeComponent();
            listView2.Visible = false;
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
            buffer = Path.Combine(textBox1.Text, listView1.SelectedItems.ToString());
            bufferItem = listView1.SelectedItems.ToString();

        }

        private void OpenDirectory()
        {
            listView1.Invoke(new Action(()=> {
            listView1.Items.Clear();
            }));
            directoryInfo = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] directory = directoryInfo.GetDirectories();
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var item in directory)
            {
                listView1.Invoke(new Action(() => {
                listView1.Items.Add(item.ToString());
                }));
            }
            foreach (var Ifile in files)
            {
                listView1.Invoke(new Action(() => {
                listView1.Items.Add(Ifile.ToString());
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenDirectory();
            
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                listView2.Visible = true;
                listView2.Clear();
                listView2.View = View.Details;
                listView2.GridLines = true;
                listView2.Columns.Add("Name");
                listView2.Columns.Add("Last Access");
                listView2.Columns.Add("Creation Date");
                listView2.Columns.Add("Extension");
                DirectoryInfo directory = new DirectoryInfo(textBox1.Text);
                foreach (var dir in directory.GetDirectories())
                {
                    ListViewItem listViewItem = new ListViewItem(dir.Name);
                    listViewItem.SubItems.Add(directoryInfo.LastAccessTime.ToShortTimeString());
                    listViewItem.SubItems.Add(directoryInfo.CreationTime.ToShortDateString());
                    listView2.Items.Add(listViewItem);
                }
                foreach (var file in directory.GetFiles())
                {

                    ListViewItem listViewItem = new ListViewItem(file.Name);
                    listViewItem.SubItems.Add(file.LastAccessTime.ToShortTimeString());
                    listViewItem.SubItems.Add(file.CreationTime.ToShortTimeString());
                    listViewItem.SubItems.Add(file.Length.ToString());
                    listViewItem.SubItems.Add(file.Extension);
                    listView2.Items.Add(listViewItem);
                }

            }
            else listView2.Visible = false;
        }

        

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)))
            {
               
                textBox1.Text = Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                listView1.Items.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] directory = directoryInfo.GetDirectories();
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (var item in directory)
                {
                    listView1.Items.Add(item.ToString());
                }
                foreach (var Ifile in files)
                {
                    listView1.Items.Add(Ifile.ToString());
                }

            }
            else if (File.Exists(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)))
            {
                Process.Start(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text));
            }
        }

        private void listView1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listView1.ContextMenuStrip = contextMenuStrip1;

            }
        }
        
    }
}
