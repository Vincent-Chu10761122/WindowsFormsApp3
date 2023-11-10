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
using Microsoft.Win32;
using System.Diagnostics;

namespace WindowsFormsApp3
{
    public partial class Form3 : Form
    {
        //private Button button1;
        public Form3()
        {
            InitializeComponent();
        }

        private void InitializeUI()
        {
            LoadFile = new Button();
            LoadFile.Text = "開啟檔案";
            LoadFile.Click += LoadFile_Click;
            Controls.Add(LoadFile);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "請選擇資料夾";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = dialog.SelectedPath;

                label1.Text = "FilePath = " + filepath;

                // 讀取資料夾中檔案名稱

                string filename = "";
                foreach (string fname in System.IO.Directory.GetFiles(filepath))
                {
                    filename = filename + fname + "\r\n";
                }
                textBox1.Text = filename;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.ShowDialog();
            this.Dispose();
            //f1.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void LoadFile_Click(object sender, EventArgs e)
        {
            /*
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "請選擇LOG檔案";
            dialog.Filter = "所有檔案(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dialog.FileName;

                label1.Text = "FileName = " + filename;
            }
            */

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "選擇要開啟的LOG檔案";
            openFileDialog.Filter = "所有檔案 (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (File.Exists(filePath))
                {
                    try
                    {
                        string fileContents = File.ReadAllText(filePath);
                        textBox1.Text = fileContents;
                    }
                    catch (Exception ex)
                    {
                        textBox1.Text = "讀取檔案時發生錯誤";
                    }
                }
                else
                {
                    textBox1.Text = "檔案不存在";
                }
            }
            else
            {
                textBox1.Text = "未選擇任何檔案";
            }


        }
    }
}
