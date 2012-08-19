namespace traqpaq_GUI
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.outLabel = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.trackIDcolumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.trackNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.recordIDcolumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.versionButton = new System.Windows.Forms.Button();
            this.readButton = new System.Windows.Forms.Button();
            this.latColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.longColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.outLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.18227F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.81773F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(808, 346);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // outLabel
            // 
            this.outLabel.AutoSize = true;
            this.outLabel.Location = new System.Drawing.Point(3, 0);
            this.outLabel.Name = "outLabel";
            this.outLabel.Size = new System.Drawing.Size(45, 13);
            this.outLabel.TabIndex = 8;
            this.outLabel.Text = "Version:";
            this.outLabel.Click += new System.EventHandler(this.versionButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.trackIDcolumnHeader,
            this.trackNameColumnHeader,
            this.recordIDcolumnHeader,
            this.dateColumnHeader,
            this.startColumnHeader,
            this.endColumnHeader,
            this.latColumnHeader,
            this.longColumnHeader});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 68);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(802, 238);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // trackIDcolumnHeader
            // 
            this.trackIDcolumnHeader.Text = "Track ID";
            // 
            // trackNameColumnHeader
            // 
            this.trackNameColumnHeader.Text = "Track Name";
            this.trackNameColumnHeader.Width = 82;
            // 
            // recordIDcolumnHeader
            // 
            this.recordIDcolumnHeader.Text = "Record ID";
            this.recordIDcolumnHeader.Width = 84;
            // 
            // dateColumnHeader
            // 
            this.dateColumnHeader.Text = "Datestamp";
            this.dateColumnHeader.Width = 89;
            // 
            // startColumnHeader
            // 
            this.startColumnHeader.Text = "Start Address";
            this.startColumnHeader.Width = 101;
            // 
            // endColumnHeader
            // 
            this.endColumnHeader.Text = "End Address";
            this.endColumnHeader.Width = 117;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.versionButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.readButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 312);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(802, 31);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // versionButton
            // 
            this.versionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionButton.Location = new System.Drawing.Point(3, 3);
            this.versionButton.Name = "versionButton";
            this.versionButton.Size = new System.Drawing.Size(94, 25);
            this.versionButton.TabIndex = 7;
            this.versionButton.Text = "Show Version";
            this.versionButton.UseVisualStyleBackColor = true;
            this.versionButton.Click += new System.EventHandler(this.versionButton_Click);
            // 
            // readButton
            // 
            this.readButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readButton.Location = new System.Drawing.Point(103, 3);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(93, 25);
            this.readButton.TabIndex = 8;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // latColumnHeader
            // 
            this.latColumnHeader.Text = "Latitude";
            this.latColumnHeader.Width = 84;
            // 
            // longColumnHeader
            // 
            this.longColumnHeader.Text = "Longitude";
            this.longColumnHeader.Width = 90;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 346);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "traq|paq";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader trackIDcolumnHeader;
        private System.Windows.Forms.ColumnHeader trackNameColumnHeader;
        private System.Windows.Forms.ColumnHeader recordIDcolumnHeader;
        private System.Windows.Forms.ColumnHeader dateColumnHeader;
        private System.Windows.Forms.ColumnHeader startColumnHeader;
        private System.Windows.Forms.ColumnHeader endColumnHeader;
        private System.Windows.Forms.ColumnHeader latColumnHeader;
        private System.Windows.Forms.ColumnHeader longColumnHeader;
        private System.Windows.Forms.Label outLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button versionButton;
        private System.Windows.Forms.Button readButton;

    }
}

