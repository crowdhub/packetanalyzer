namespace PacketAnalyzer
{
    partial class frmStatistics
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.rdoPacketPerSecond = new System.Windows.Forms.RadioButton();
            this.rdoTrafficPerProtocol = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(16, 99);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(805, 533);
            this.chart.TabIndex = 2;
            this.chart.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Line",
            "Spline",
            "StepLine",
            "FastLine",
            "Bar",
            "StackedBar",
            "StackedBar100",
            "Column",
            "StackedColumn",
            "StackedColumn100",
            "Area",
            "SplineArea",
            "StackedArea",
            "StackedArea100",
            "Pie",
            "Doughnut",
            "Stock",
            "Candlestick",
            "Range",
            "SplineRange",
            "RangeBar",
            "RangeColumn",
            "Radar",
            "Polar",
            "ErrorBar",
            "BoxPlot",
            "Renko",
            "ThreeLineBreak",
            "Kagi",
            "PointAndFigure",
            "Funnel",
            "Pyramid"});
            this.comboBox1.Location = new System.Drawing.Point(827, 99);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(164, 23);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // rdoPacketPerSecond
            // 
            this.rdoPacketPerSecond.AutoSize = true;
            this.rdoPacketPerSecond.Location = new System.Drawing.Point(16, 20);
            this.rdoPacketPerSecond.Name = "rdoPacketPerSecond";
            this.rdoPacketPerSecond.Size = new System.Drawing.Size(152, 19);
            this.rdoPacketPerSecond.TabIndex = 4;
            this.rdoPacketPerSecond.TabStop = true;
            this.rdoPacketPerSecond.Text = "Packet per second";
            this.rdoPacketPerSecond.UseVisualStyleBackColor = true;
            this.rdoPacketPerSecond.CheckedChanged += new System.EventHandler(this.rdoPacketPerSecond_CheckedChanged);
            // 
            // rdoTrafficPerProtocol
            // 
            this.rdoTrafficPerProtocol.AutoSize = true;
            this.rdoTrafficPerProtocol.Location = new System.Drawing.Point(16, 46);
            this.rdoTrafficPerProtocol.Name = "rdoTrafficPerProtocol";
            this.rdoTrafficPerProtocol.Size = new System.Drawing.Size(185, 19);
            this.rdoTrafficPerProtocol.TabIndex = 5;
            this.rdoTrafficPerProtocol.TabStop = true;
            this.rdoTrafficPerProtocol.Text = "Traffic for each protocol";
            this.rdoTrafficPerProtocol.UseVisualStyleBackColor = true;
            this.rdoTrafficPerProtocol.CheckedChanged += new System.EventHandler(this.rdoTrafficPerProtocol_CheckedChanged);
            // 
            // frmStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 644);
            this.Controls.Add(this.rdoTrafficPerProtocol);
            this.Controls.Add(this.rdoPacketPerSecond);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStatistics";
            this.Text = "Statistics";
            this.Load += new System.EventHandler(this.frmStatistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton rdoPacketPerSecond;
        private System.Windows.Forms.RadioButton rdoTrafficPerProtocol;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}