﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Globalization;

namespace WindowsFormsApp3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();          
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            //Voltage.SelectedIndex = 0;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //Voltage.SelectedIndex = 1;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-10).ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisY.Minimum = 0; //設定Y軸最小值
            chart1.ChartAreas[0].AxisY.Maximum = 100; //設定Y軸最大值
            chart1.ChartAreas[0].AxisY.Interval = 10; //設定Y軸刻度間距

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true; //設定X軸開啟縮放功能
            //chart1.ChartAreas[0].AxisX.ScaleView.Size = 10; //設定初始可見區域的時間跨度
            //chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 10; //設定最小可見的時間跨度

            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true; //設定Y軸開啟縮放功能
            chart1.ChartAreas[0].AxisY.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart1.ChartAreas[0].AxisY.ScaleView.Size = 20;
            chart1.ChartAreas[0].AxisY.ScaleView.MinSize = 1;
            //chart1.ChartAreas[0].AxisY.Minimum = 30; //設定Y軸最小值

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件 (*.txt)|*.txt";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                chart1.Series["電量"].Points.Clear();
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                List<string[]> dataList = new List<string[]>();
                foreach(string line in lines)
                {
                    if(line.Length >= 70 && line.Length <= 74 && line.Contains(","))
                    {
                        //Console.WriteLine(line);
                        string[] parts = line.Replace(",", "").Split(' ');
                        dataList.Add(parts);
                    }
                }

                dataList.Sort((a, b) => DateTime.ParseExact(a[1], "HH:mm:ss.ffffff", null).CompareTo(DateTime.ParseExact(b[1], "HH:mm:ss.ffffff", null)));
                
                foreach (string[] data in dataList) //電量
                {

                    if (data.Length > 1)
                    {
                        if (DateTime.TryParseExact(data[1], "HH:mm:ss.ffffff", null, System.Globalization.DateTimeStyles.None, out DateTime xValue) && double.TryParse(data[6], out double yValue))
                        {
                            chart1.Series["電量"].Points.AddXY(xValue, yValue);
                        }
                        else
                        {
                            Console.WriteLine("無法轉換為DateTime: " + data[1] + " 或雙精度浮點數: " + data[6]);
                        }
                    }
                }

                if(chart1.Series["電量"].Points.Count > 25)
                {
                    double xMin = chart1.Series["電量"].Points[chart1.Series["電量"].Points.Count - 25].XValue;
                    double xMax = chart1.Series["電量"].Points[chart1.Series["電量"].Points.Count - 1].XValue;

                    chart1.ChartAreas[0].AxisX.Minimum = xMin;
                    chart1.ChartAreas[0].AxisX.Maximum = xMax;
                }

                if (chart1.Series["電量"].Points.Count > 10)
                {
                    double yMin = chart1.Series["電量"].Points[chart1.Series["電量"].Points.Count - 10].YValues[0];
                    double yMax = chart1.Series["電量"].Points[chart1.Series["電量"].Points.Count - 1].YValues[0];

                    chart1.ChartAreas[0].AxisX.Minimum = yMin;
                    chart1.ChartAreas[0].AxisX.Maximum = yMax;
                }

                //設定X軸Label的間隔 : 2023/11/10 每分鐘顯示一個
                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chart1.ChartAreas[0].AxisX.Interval = 1;

                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss"; //設定X軸的Label顯示的格式

                //調整Y軸範圍
                chart1.ChartAreas[0].AxisY.Minimum = chart1.Series["電量"].Points.Select(point => point.YValues[0]).Min();
                chart1.ChartAreas[0].AxisY.Maximum = chart1.Series["電量"].Points.Select(point => point.YValues[0]).Max();

                //調整X軸範圍
                chart1.ChartAreas[0].AxisX.Minimum = chart1.Series["電量"].Points.First().XValue;
                chart1.ChartAreas[0].AxisX.Maximum = chart1.Series["電量"].Points.Last().XValue;

                int numberOfData = dataList.Count;
                Console.WriteLine("Number of data rows: " + numberOfData);
            }

            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series["電量"].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = chart1.Series["電量"].Points[chart1.Series["電量"].Points.Count - 1].XValue;

            chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();

            chart1.MouseWheel += Chart1_MouseWheel;
        }

        private Stack <Tuple<double, double>> zoomStack = new Stack <Tuple<double, double>>();
        private int backwardScrollCount = 0;
        private void Chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            ChartArea chartArea = chart.ChartAreas[0];
            double xMin = chartArea.AxisX.ScaleView.ViewMinimum;
            double xMax = chartArea.AxisX.ScaleView.ViewMaximum;

            if (e.Delta < 0) //檢查滑鼠滾輪方向
            {
                //向後滾動縮小
                backwardScrollCount++;
                RestorePreviousZoom();
                //chartArea.AxisX.ScaleView.ZoomReset();
            }
            else
            {
                //向前滾動放大
                double zoomFactor = 0.9; //調整放大倍數
                double xPosition = chartArea.AxisX.PixelPositionToValue(e.Location.X);
                double newXMin = xPosition - (xPosition - xMin) * zoomFactor;
                double newXMax = xPosition + (xMax - xPosition) * zoomFactor;

                chartArea.AxisX.ScaleView.Zoom(newXMin, newXMax);

                zoomStack.Push(new Tuple<double, double>(xMin, xMax)); //把目前的ViewMinimum和ViewMaximum加入堆疊
            }
        }
        private void RestorePreviousZoom() //用於在需要的地方恢復為前一次的大小
        {
            if(zoomStack.Count > 0)
            {
                Tuple<double, double> previousZoom = zoomStack.Pop();
                chart1.ChartAreas[0].AxisX.ScaleView.Zoom(previousZoom.Item1, previousZoom.Item2);
                backwardScrollCount--;
            }
        }

        //private bool isDraggingXAxis = false;
        /*
        private void Chart1_AxisViewChanged(object sender, ViewEventArgs e)
        {
            Chart chart = (Chart)sender;
            ChartArea chartArea = chart.ChartAreas[0];

            int maxDataPoints = 10;
            double minView = chartArea.AxisX.ScaleView.ViewMinimum;
            double maxView = chartArea.AxisX.ScaleView.ViewMaximum;
            double dataSize = maxView - minView;

            if(dataSize > maxDataPoints)
            {
                chartArea.AxisX.ScaleView.Zoom(maxView - maxDataPoints, maxView);
            }

            if (isDraggingXAxis)
                return;
        }
        */

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
