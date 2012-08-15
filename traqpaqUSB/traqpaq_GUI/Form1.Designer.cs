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
            this.versionButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.outLabel = new System.Windows.Forms.Label();
            this.readRecordTableButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // versionButton
            // 
            this.versionButton.Location = new System.Drawing.Point(12, 227);
            this.versionButton.Name = "versionButton";
            this.versionButton.Size = new System.Drawing.Size(109, 23);
            this.versionButton.TabIndex = 0;
            this.versionButton.Text = "Show Version";
            this.versionButton.UseVisualStyleBackColor = true;
            this.versionButton.Click += new System.EventHandler(this.versionButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(208, 227);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // outLabel
            // 
            this.outLabel.AutoSize = true;
            this.outLabel.Location = new System.Drawing.Point(9, 9);
            this.outLabel.Name = "outLabel";
            this.outLabel.Size = new System.Drawing.Size(45, 13);
            this.outLabel.TabIndex = 2;
            this.outLabel.Text = "Version:";
            // 
            // readRecordTableButton
            // 
            this.readRecordTableButton.Location = new System.Drawing.Point(127, 227);
            this.readRecordTableButton.Name = "readRecordTableButton";
            this.readRecordTableButton.Size = new System.Drawing.Size(75, 23);
            this.readRecordTableButton.TabIndex = 3;
            this.readRecordTableButton.Text = "Read";
            this.readRecordTableButton.UseVisualStyleBackColor = true;
            this.readRecordTableButton.Click += new System.EventHandler(this.readRecordTableButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(8, 117);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(487, 97);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Saved Runs";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Track ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Record ID";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Datestamp";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Start Address";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "End Address";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Track Name";
            this.columnHeader6.Width = 82;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.readRecordTableButton);
            this.Controls.Add(this.outLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.versionButton);
            this.Name = "Form1";
            this.Text = "traq|paq";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button versionButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label outLabel;
        private System.Windows.Forms.Button readRecordTableButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}

