using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private int m_nTimeCount = 0;
        Random clsRadom = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            //this.chart1.Series.Clear();
            ChartArea chtArea = new ChartArea("ViewArea");
            chtArea.AxisX.Minimum = 0;
            chtArea.AxisX.ScaleView.Size = 10;
            chtArea.AxisX.Interval = 5;
            chtArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chtArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0] = chtArea;

            Timer clsTimer = new Timer();
            clsTimer.Tick += new EventHandler(RefreshChart);
            clsTimer.Interval = 300;
            clsTimer.Start();
        }

        private void RefreshChart(object sender, EventArgs e)
        {
            chart1.Series[0].Points.AddXY(m_nTimeCount, clsRadom.Next(0, 100));
            if(m_nTimeCount > 10)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Position = m_nTimeCount - 10;
            }
            m_nTimeCount++;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            //this.chart1.Series.Clear();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
