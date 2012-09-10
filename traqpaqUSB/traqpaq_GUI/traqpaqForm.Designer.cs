namespace traqpaq_GUI
{
    partial class traqpaqForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderTrackID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTrackName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRecordID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStartAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEndAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLatitude = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLongitude = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOutputKML = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonOutputCSV = new System.Windows.Forms.Button();
            this.buttonPlotText = new System.Windows.Forms.Button();
            this.buttonOutputKMLfromFile = new System.Windows.Forms.Button();
            this.buttonPlot = new System.Windows.Forms.Button();
            this.buttonGoogleEarth = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGoogleEarth = new System.Windows.Forms.TabPage();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonPlotGPS = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.34053F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.65947F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(696, 444);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTrackID,
            this.columnHeaderTrackName,
            this.columnHeaderRecordID,
            this.columnHeaderDate,
            this.columnHeaderStartAddr,
            this.columnHeaderEndAddr,
            this.columnHeaderLatitude,
            this.columnHeaderLongitude});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 306);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(690, 98);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTrackID
            // 
            this.columnHeaderTrackID.Text = "Track ID";
            // 
            // columnHeaderTrackName
            // 
            this.columnHeaderTrackName.Text = "Track Name";
            this.columnHeaderTrackName.Width = 82;
            // 
            // columnHeaderRecordID
            // 
            this.columnHeaderRecordID.Text = "Record ID";
            this.columnHeaderRecordID.Width = 84;
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Datestamp";
            this.columnHeaderDate.Width = 89;
            // 
            // columnHeaderStartAddr
            // 
            this.columnHeaderStartAddr.Text = "Start Address";
            this.columnHeaderStartAddr.Width = 101;
            // 
            // columnHeaderEndAddr
            // 
            this.columnHeaderEndAddr.Text = "End Address";
            this.columnHeaderEndAddr.Width = 117;
            // 
            // columnHeaderLatitude
            // 
            this.columnHeaderLatitude.Text = "Latitude";
            this.columnHeaderLatitude.Width = 79;
            // 
            // columnHeaderLongitude
            // 
            this.columnHeaderLongitude.Text = "Longitude";
            this.columnHeaderLongitude.Width = 74;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOutputKML, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonTest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOutputCSV, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPlotText, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOutputKMLfromFile, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPlot, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonGoogleEarth, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPlotGPS, 7, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 410);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(690, 31);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // buttonOutputKML
            // 
            this.buttonOutputKML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOutputKML.Location = new System.Drawing.Point(128, 3);
            this.buttonOutputKML.Name = "buttonOutputKML";
            this.buttonOutputKML.Size = new System.Drawing.Size(78, 25);
            this.buttonOutputKML.TabIndex = 11;
            this.buttonOutputKML.Text = "Output KML";
            this.buttonOutputKML.UseVisualStyleBackColor = true;
            this.buttonOutputKML.Click += new System.EventHandler(this.buttonOutputKML_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTest.Location = new System.Drawing.Point(3, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(42, 25);
            this.buttonTest.TabIndex = 7;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonOutputCSV
            // 
            this.buttonOutputCSV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOutputCSV.Location = new System.Drawing.Point(51, 3);
            this.buttonOutputCSV.Name = "buttonOutputCSV";
            this.buttonOutputCSV.Size = new System.Drawing.Size(71, 25);
            this.buttonOutputCSV.TabIndex = 8;
            this.buttonOutputCSV.Text = "Output CSV";
            this.buttonOutputCSV.UseVisualStyleBackColor = true;
            this.buttonOutputCSV.Click += new System.EventHandler(this.buttonOutputCSV_Click);
            // 
            // buttonPlotText
            // 
            this.buttonPlotText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlotText.Location = new System.Drawing.Point(212, 3);
            this.buttonPlotText.Name = "buttonPlotText";
            this.buttonPlotText.Size = new System.Drawing.Size(84, 25);
            this.buttonPlotText.TabIndex = 9;
            this.buttonPlotText.Text = "Plot From Text";
            this.buttonPlotText.UseVisualStyleBackColor = true;
            this.buttonPlotText.Click += new System.EventHandler(this.buttonPlotText_Click);
            // 
            // buttonOutputKMLfromFile
            // 
            this.buttonOutputKMLfromFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOutputKMLfromFile.Location = new System.Drawing.Point(302, 3);
            this.buttonOutputKMLfromFile.Name = "buttonOutputKMLfromFile";
            this.buttonOutputKMLfromFile.Size = new System.Drawing.Size(97, 25);
            this.buttonOutputKMLfromFile.TabIndex = 12;
            this.buttonOutputKMLfromFile.Text = "Output KMLfile";
            this.buttonOutputKMLfromFile.UseVisualStyleBackColor = true;
            this.buttonOutputKMLfromFile.Click += new System.EventHandler(this.buttonOutputKMLfromFile_Click);
            // 
            // buttonPlot
            // 
            this.buttonPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlot.Location = new System.Drawing.Point(405, 3);
            this.buttonPlot.Name = "buttonPlot";
            this.buttonPlot.Size = new System.Drawing.Size(63, 25);
            this.buttonPlot.TabIndex = 13;
            this.buttonPlot.Text = "Plot Chart";
            this.buttonPlot.UseVisualStyleBackColor = true;
            this.buttonPlot.Click += new System.EventHandler(this.buttonPlot_Click);
            // 
            // buttonGoogleEarth
            // 
            this.buttonGoogleEarth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGoogleEarth.Location = new System.Drawing.Point(474, 3);
            this.buttonGoogleEarth.Name = "buttonGoogleEarth";
            this.buttonGoogleEarth.Size = new System.Drawing.Size(56, 25);
            this.buttonGoogleEarth.TabIndex = 14;
            this.buttonGoogleEarth.Text = "Plot GE";
            this.buttonGoogleEarth.UseVisualStyleBackColor = true;
            this.buttonGoogleEarth.Click += new System.EventHandler(this.buttonGoogleEarth_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGoogleEarth);
            this.tabControl1.Controls.Add(this.tabPageChart);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(690, 297);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPageGoogleEarth
            // 
            this.tabPageGoogleEarth.Location = new System.Drawing.Point(4, 22);
            this.tabPageGoogleEarth.Name = "tabPageGoogleEarth";
            this.tabPageGoogleEarth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGoogleEarth.Size = new System.Drawing.Size(682, 271);
            this.tabPageGoogleEarth.TabIndex = 1;
            this.tabPageGoogleEarth.Text = "Google Earth";
            this.tabPageGoogleEarth.UseVisualStyleBackColor = true;
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.chart1);
            this.tabPageChart.Location = new System.Drawing.Point(4, 22);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChart.Size = new System.Drawing.Size(682, 271);
            this.tabPageChart.TabIndex = 0;
            this.tabPageChart.Text = "Chart";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(676, 265);
            this.chart1.TabIndex = 17;
            this.chart1.Text = "chart1";
            // 
            // buttonPlotGPS
            // 
            this.buttonPlotGPS.Location = new System.Drawing.Point(536, 3);
            this.buttonPlotGPS.Name = "buttonPlotGPS";
            this.buttonPlotGPS.Size = new System.Drawing.Size(75, 23);
            this.buttonPlotGPS.TabIndex = 15;
            this.buttonPlotGPS.Text = "Plot GPS";
            this.buttonPlotGPS.UseVisualStyleBackColor = true;
            this.buttonPlotGPS.Click += new System.EventHandler(this.buttonPlotGPS_Click);
            // 
            // traqpaqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 444);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "traqpaqForm";
            this.Text = "traq|paq";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderTrackID;
        private System.Windows.Forms.ColumnHeader columnHeaderTrackName;
        private System.Windows.Forms.ColumnHeader columnHeaderRecordID;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderStartAddr;
        private System.Windows.Forms.ColumnHeader columnHeaderEndAddr;
        private System.Windows.Forms.ColumnHeader columnHeaderLatitude;
        private System.Windows.Forms.ColumnHeader columnHeaderLongitude;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonOutputCSV;
        private System.Windows.Forms.Button buttonPlotText;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOutputKML;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonOutputKMLfromFile;
        private System.Windows.Forms.Button buttonPlot;
        private System.Windows.Forms.Button buttonGoogleEarth;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageChart;
        private System.Windows.Forms.TabPage tabPageGoogleEarth;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button buttonPlotGPS;

    }
}

