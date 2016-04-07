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

namespace PacketAnalyzer
{
    public partial class frmStatistics : Form
    {
        private OCurrentPacketList _list = null;
        private OStatistics.StatisticsType _curType = OStatistics.StatisticsType.Uknown;

        private void _UpdateStatistics()
        {
            OStatistics.StatisticsType newType;
            if (true == rdoPacketPerSecond.Checked)
            {
                newType = OStatistics.StatisticsType.PacketPerSecond;
            }
            else
            {
                newType = OStatistics.StatisticsType.TrafficPerProtocol;
            }
            if (newType == _curType)
            {
                return;
            }

            OStatistics stat;
            if (newType == OStatistics.StatisticsType.PacketPerSecond)
            {
                stat = new OStatisticsPacketPerTime();
            }
            else if (newType == OStatistics.StatisticsType.TrafficPerProtocol)
            {
                stat = new OStatisticsPacketPerProtocol();
            }
            else
            {
                MessageBox.Show("Can't recognize statistics type", "Error");
                return;
            }

            // Calculate new statistics type
            OStatisticsResult res = stat.CalculateStatistics(_list, newType);
            if (null == res)
            {
                MessageBox.Show("Failed to calculate statistics", "Error");
                return;
            }

            _curType = newType;

            // Clear all data before adding new statistics data
            chart.Series[0].Points.Clear();
            if (_curType == OStatistics.StatisticsType.PacketPerSecond) chart.Series[0].Name = "Packets per second";
            else if (_curType == OStatistics.StatisticsType.TrafficPerProtocol) chart.Series[0].Name = "Packets per protocol";

            if (res.GetItemCount() <= 0)
            {
                MessageBox.Show("There is no result", "Information");
                return;
            }

            // Update chart based on the result
            Dictionary<string, int>.Enumerator _enumerator = res.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                string name = _enumerator.Current.Key;
                int value = _enumerator.Current.Value;
                chart.Series[0].Points.AddXY(name, value);
            }
        }

        public frmStatistics()
        {
            InitializeComponent();
        }
        public void SetPacketList(OCurrentPacketList curList)
        {
            _list = curList;
        }

        private void frmStatistics_Load(object sender, EventArgs e)
        {
            // make default value to Pie chart
            comboBox1.SelectedIndex = 14;
            // default: Packet per second
            rdoPacketPerSecond.Checked = true;
            _UpdateStatistics();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nIndex = comboBox1.SelectedIndex;
            if (nIndex < 0 || nIndex >= comboBox1.Items.Count)
            {
                return;
            }

            SeriesChartType chartType = SeriesChartType.Line;

            if (0 == nIndex) chartType = SeriesChartType.Line;
            else if (1 == nIndex) chartType = SeriesChartType.Spline;
            else if (2 == nIndex) chartType = SeriesChartType.StepLine;
            else if (3 == nIndex) chartType = SeriesChartType.FastLine;
            else if (4 == nIndex) chartType = SeriesChartType.Bar;
            else if (5 == nIndex) chartType = SeriesChartType.StackedBar;
            else if (6 == nIndex) chartType = SeriesChartType.StackedBar100;
            else if (7== nIndex) chartType = SeriesChartType.Column;
            else if (8 == nIndex) chartType = SeriesChartType.StackedColumn;
            else if (9 == nIndex) chartType = SeriesChartType.StackedColumn100;
            else if (10 == nIndex) chartType = SeriesChartType.Area;
            else if (11 == nIndex) chartType = SeriesChartType.SplineArea;
            else if (12 == nIndex) chartType = SeriesChartType.StackedArea;
            else if (13 == nIndex) chartType = SeriesChartType.StackedArea100;
            else if (14 == nIndex) chartType = SeriesChartType.Pie;
            else if (15 == nIndex) chartType = SeriesChartType.Doughnut;
            else if (16 == nIndex) chartType = SeriesChartType.Stock;
            else if (17 == nIndex) chartType = SeriesChartType.Candlestick;
            else if (18 == nIndex) chartType = SeriesChartType.Range;
            else if (19 == nIndex) chartType = SeriesChartType.SplineRange;
            else if (20 == nIndex) chartType = SeriesChartType.RangeBar;
            else if (21 == nIndex) chartType = SeriesChartType.RangeColumn;
            else if (22 == nIndex) chartType = SeriesChartType.Radar;
            else if (23 == nIndex) chartType = SeriesChartType.Polar;
            else if (24 == nIndex) chartType = SeriesChartType.ErrorBar;
            else if (25 == nIndex) chartType = SeriesChartType.BoxPlot;
            else if (26 == nIndex) chartType = SeriesChartType.Renko;
            else if (27 == nIndex) chartType = SeriesChartType.ThreeLineBreak;
            else if (28 == nIndex) chartType = SeriesChartType.Kagi;
            else if (29 == nIndex) chartType = SeriesChartType.PointAndFigure;
            else if (30 == nIndex) chartType = SeriesChartType.Funnel;
            else if (31 == nIndex) chartType = SeriesChartType.Pyramid;

            for (int i=0; i<chart.Series.Count; i++)
            {
                chart.Series[i].ChartType = chartType;
            }
        }

        private void rdoPacketPerSecond_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (null != rb)
            {
                if (rb.Checked)
                {
                    _UpdateStatistics();
                }
            }
        }

        private void rdoTrafficPerProtocol_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (null != rb)
            {
                if (rb.Checked)
                {
                    _UpdateStatistics();
                }
            }
        }
    }
}
