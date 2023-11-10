using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.IO;

namespace WindowsFormsApp3
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }

    }

    /*
    class LoadData
    {
        public void DataSet()
        {
            string[] lines = File.ReadAllLines("file_path.txt");

            List<int[]> data = new List<int[]>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');

                int[] values = new int[parts.Length];

                for (int i=0; i<parts.Length; i++)
                {
                    if(int.TryParse(parts[i], out int value))
                    {
                        values[i] = value;
                    }

                    else
                    {
                        Console.WriteLine("無法解析內容: " + parts[i]);
                    }
                }

                data.Add(values);
            }

            int value00 = data[0][0];
            Console.WriteLine("第一行第一個值: " + value00);

            return ;
        }
    }
    */
}
