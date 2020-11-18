namespace SouthernAbstractConverter
{
    partial class frmConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConverter));
            this.btnReadFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.txtBoxFilePath = new System.Windows.Forms.TextBox();
            this.txtBoxOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DatabaseFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.dbFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.lblPrgBar1 = new System.Windows.Forms.Label();
            this.lblPrgBar2 = new System.Windows.Forms.Label();
            this.preprocesing = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnReadFiles
            // 
            this.btnReadFiles.Location = new System.Drawing.Point(606, 10);
            this.btnReadFiles.Name = "btnReadFiles";
            this.btnReadFiles.Size = new System.Drawing.Size(105, 23);
            this.btnReadFiles.TabIndex = 3;
            this.btnReadFiles.Text = "Import Files";
            this.btnReadFiles.UseVisualStyleBackColor = true;
            this.btnReadFiles.Click += new System.EventHandler(this.btnReadFiles_Click);
            this.btnReadFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button1_MouseUp);
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Location = new System.Drawing.Point(487, 10);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(113, 23);
            this.btnFileBrowser.TabIndex = 2;
            this.btnFileBrowser.Tag = "Files to convert";
            this.btnFileBrowser.Text = "Select File Path";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            this.btnFileBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnFileBrowser_MouseUp);
            // 
            // txtBoxFilePath
            // 
            this.txtBoxFilePath.Location = new System.Drawing.Point(117, 12);
            this.txtBoxFilePath.Name = "txtBoxFilePath";
            this.txtBoxFilePath.Size = new System.Drawing.Size(360, 20);
            this.txtBoxFilePath.TabIndex = 0;
            this.txtBoxFilePath.Text = "C:\\SoutherAbs_data";
            // 
            // txtBoxOutput
            // 
            this.txtBoxOutput.Location = new System.Drawing.Point(12, 67);
            this.txtBoxOutput.Multiline = true;
            this.txtBoxOutput.Name = "txtBoxOutput";
            this.txtBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxOutput.Size = new System.Drawing.Size(942, 205);
            this.txtBoxOutput.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Data Files Path:";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Transparent;
            this.progressBar1.Location = new System.Drawing.Point(15, 302);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(939, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblStatus.Location = new System.Drawing.Point(789, 44);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(19, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "...";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(717, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(745, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Status:";
            // 
            // DatabaseFile
            // 
            this.DatabaseFile.Location = new System.Drawing.Point(486, 36);
            this.DatabaseFile.Name = "DatabaseFile";
            this.DatabaseFile.Size = new System.Drawing.Size(113, 25);
            this.DatabaseFile.TabIndex = 9;
            this.DatabaseFile.Text = "Locate Database";
            this.DatabaseFile.UseVisualStyleBackColor = true;
            this.DatabaseFile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DatabaseFile_MouseUp);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"Database files (*.accdb)|*.accdb|All files (*.*)|*.*\"";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(21, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Database Path:";
            // 
            // dbFilePath
            // 
            this.dbFilePath.Location = new System.Drawing.Point(116, 41);
            this.dbFilePath.Name = "dbFilePath";
            this.dbFilePath.Size = new System.Drawing.Size(360, 20);
            this.dbFilePath.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(15, 275);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 24);
            this.label4.TabIndex = 12;
            this.label4.Text = "File Progress";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(15, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 24);
            this.label5.TabIndex = 13;
            this.label5.Text = "Overall Progress";
            // 
            // progressBar2
            // 
            this.progressBar2.ForeColor = System.Drawing.Color.Transparent;
            this.progressBar2.Location = new System.Drawing.Point(15, 351);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(936, 22);
            this.progressBar2.TabIndex = 14;
            // 
            // lblPrgBar1
            // 
            this.lblPrgBar1.AutoSize = true;
            this.lblPrgBar1.BackColor = System.Drawing.Color.Transparent;
            this.lblPrgBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrgBar1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPrgBar1.Location = new System.Drawing.Point(95, 275);
            this.lblPrgBar1.Name = "lblPrgBar1";
            this.lblPrgBar1.Size = new System.Drawing.Size(16, 13);
            this.lblPrgBar1.TabIndex = 15;
            this.lblPrgBar1.Text = "%";
            // 
            // lblPrgBar2
            // 
            this.lblPrgBar2.AutoSize = true;
            this.lblPrgBar2.BackColor = System.Drawing.Color.Transparent;
            this.lblPrgBar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrgBar2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPrgBar2.Location = new System.Drawing.Point(114, 328);
            this.lblPrgBar2.Name = "lblPrgBar2";
            this.lblPrgBar2.Size = new System.Drawing.Size(16, 13);
            this.lblPrgBar2.TabIndex = 16;
            this.lblPrgBar2.Text = "%";
            // 
            // preprocesing
            // 
            this.preprocesing.BackColor = System.Drawing.Color.Transparent;
            this.preprocesing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preprocesing.ForeColor = System.Drawing.SystemColors.Control;
            this.preprocesing.Location = new System.Drawing.Point(607, 27);
            this.preprocesing.Name = "preprocesing";
            this.preprocesing.Size = new System.Drawing.Size(132, 44);
            this.preprocesing.TabIndex = 17;
            this.preprocesing.Text = "Enable File Processor";
            this.preprocesing.UseVisualStyleBackColor = false;
            this.preprocesing.CheckedChanged += new System.EventHandler(this.preprocesing_CheckedChanged);
            // 
            // frmConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(975, 400);
            this.Controls.Add(this.lblPrgBar1);
            this.Controls.Add(this.lblPrgBar2);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dbFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DatabaseFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxOutput);
            this.Controls.Add(this.txtBoxFilePath);
            this.Controls.Add(this.btnFileBrowser);
            this.Controls.Add(this.btnReadFiles);
            this.Controls.Add(this.preprocesing);
            this.Name = "frmConverter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConverter_FormClosing);
            this.Load += new System.EventHandler(this.frmConverter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.CheckBox preprocesing;

        private System.Windows.Forms.ProgressBar progressBar2;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.TextBox dbFilePath;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.OpenFileDialog openFileDialog1;

        private System.Windows.Forms.Button DatabaseFile;

        #endregion

        private System.Windows.Forms.Button btnReadFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.TextBox txtBoxFilePath;
        private System.Windows.Forms.TextBox txtBoxOutput;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPrgBar1;
        private System.Windows.Forms.Label lblPrgBar2;
    }
}

