using System;
//using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
//using Application = Microsoft.Office.Interop.Word.Application;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SouthernAbstractConverter.Service;

namespace SouthernAbstractConverter
{
    public partial class frmConverter : Form
    {
        StringBuilder stringBuild = new StringBuilder();
        int progress;
        public frmConverter()
        {
            InitializeComponent();

            //Setup Background Worker (multi thread)
            //backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            //backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            //backgroundWorker1.RunWorkerCompleted +=
            //    backgroundWorker1_RunWorkerCompleted; //Tell the user how the process went
            //backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
        }
        //automated Tests
        public frmConverter(string[] myargs)
        {
            InitializeComponent();

            //Setup Background Worker (multi thread)
            //backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            //backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            //backgroundWorker1.RunWorkerCompleted +=
            //    backgroundWorker1_RunWorkerCompleted; //Tell the user how the process went
            //backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
            if (myargs.Contains("-test"))
            {
                var databaseOriginal = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.accdb").First();
                var newrfile = databaseOriginal.FullName.Replace(".accdb", "2.accdb");
                File.Copy(databaseOriginal.FullName,newrfile,true);
                Service.SADataService.UpdateDatabaseString(newrfile);
                var DatafilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles");
                txtBoxFilePath.Text = DatafilePath;
                ThreadGlobals.ShouldPreprocess = true;
                StartProcesing();
                
            }
        }

        #region Background Worker

                private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Service.SADataService saData = new Service.SADataService();
            //Because this object exists on the main thread I have to create it here on the backgroundworker thread
            TextBox txtBoxOutput = new TextBox();

            // txtBoxOutput.Text = saData.ConvertText(@"c:\data\pg02.txt");
            //saData = null;

            //add all files in the directory to the array
            string textfield;

            //const string V = @"\";

            //textfield = Path.GetFullPath(this.txtBoxFilePath.Text).Replace("\\", V);
            textfield = Path.GetFullPath(this.txtBoxFilePath.Text);
            var arrayFilesAndPath = Directory.EnumerateFiles(textfield, "*.txt").ToList();
            //string[] arrayFilesAndPath = Directory.GetFiles(@"C:\", "*.txt");
            if (arrayFilesAndPath.Count != 0)
            {
                //for every file found import/convert into MS Access Database
                for (var i = 0; i < arrayFilesAndPath.Count; i++)
                {
                    //Calculate percentage complete
                    if (i == 0)
                    {
                        //Convert integer to float
                        float f = (((float) i / (float) arrayFilesAndPath.Count) * 100f);
                        progress = (int) f;
                        backgroundWorker1.ReportProgress((int) progress);
                    }

                    //Check if there is a request to cancel the process
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        backgroundWorker1.ReportProgress(progressBar1.Value);
                        return;
                    }

                    /* Output the File and Path */
                    txtBoxOutput.Text += arrayFilesAndPath[i];

                    if (txtBoxOutput.TextLength != 0)
                    {
                        backgroundWorker1.ReportProgress(progress, txtBoxOutput);
                    }

                    /* Begin Conversion */
                    //txtBoxOutput.Text = saData.ConvertText(arrayFilesAndPath[i]);
                    saData.ConvertText(arrayFilesAndPath[i], data => backgroundWorker1.ReportProgress(progress, data));
                }

                saData = null;

                //If the process exits the loop, ensure that progress is set to 100%
                //Remember in the loop we set i < 100 so in theory the process will complete at 99%
                SADataService.DataInsertion.CloseConnection();
                txtBoxOutput.Text = "Copying data to database complete";
                backgroundWorker1.ReportProgress(100, txtBoxOutput);
            }
            else
            {
                txtBoxOutput.Text = "Text Files not found in specified path";
                backgroundWorker1.ReportProgress(100, txtBoxOutput);
            }
        }

        //Thread Progress
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            if (progressBar1.Value == 100)
            {
                this.btnReadFiles.Enabled = true;
            }

            if (e.UserState != null)
            {
                /* Send the output message to the user */
                string status = e.UserState.ToString();
                status = status.Trim();
                try
                {
                    status = status.Remove(0, 36);
                }
                catch (Exception exception)
                {
                    //Console.WriteLine(exception);
                    //throw;
                }
                /*System.Windows.Forms.TextBox, Text*/


                stringBuild.AppendLine(status);
                this.txtBoxOutput.Text = stringBuild.ToString();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender,
            System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.lblStatus = new Label();

            if (e.Cancelled)
            {
                this.lblStatus.Text = "Process was cancelled";
            }
            else if (e.Error != null)
            {
                this.lblStatus.Text = "There was an error running the process. The thread aborted";
            }
            else
            {
                this.lblStatus.Text = "Process was completed";
            }
        }

        #endregion
        //Thread Cancel Process
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Check if background worker is doing anything and send a cancellation if it is
            // if (backgroundWorker1.IsBusy)
            //{
            //     this.txtBoxOutput.Text = "Cancellation requested";
            //     this.txtBoxOutput.AppendText(Environment.NewLine);
            //     backgroundWorker1.CancelAsync();
            // }

            ThreadGlobals.ShouldCancel = true;
            // this.btnReadFiles.Enabled = true;
            //this.lblStatus.Text = "Import canceled";
            //this.txtBoxOutput.AppendText("Cancellation completed");
            // backgroundWorker1.CancelAsync();
        }
        //Start the import process
        private async void button1_MouseUp(object sender, MouseEventArgs e)
        {
            await StartProcesing();
        }

        private void btnFileBrowser_MouseUp(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();

            //Display the File Browser Dialog
            fb.ShowDialog();

            //File Path to include file name
            String filePath = fb.SelectedPath;

            //Set the text box file path
            this.txtBoxFilePath.Text = filePath;
        }

        private void DatabaseFile_MouseUp(object sender, MouseEventArgs e)
        {
            //Display the File Browser Dialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //File Path to include file name
                String filePath = openFileDialog1.FileName;

                Service.SADataService.UpdateDatabaseString(filePath);
                //Set the text box file path
                this.dbFilePath.Text = filePath;
            }
        }

        private void frmConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            ThreadGlobals.ShouldCancel = true;
            SADataService.DataInsertion.CloseConnection();
        }

        private void frmConverter_Load(object sender, EventArgs e)
        {

        }

        private void btnReadFiles_Click(object sender, EventArgs e)
        {

        }

        private void preprocesing_CheckedChanged(object sender, EventArgs e)
        {
            ThreadGlobals.ShouldPreprocess = preprocesing.Checked;
        }
    }
}