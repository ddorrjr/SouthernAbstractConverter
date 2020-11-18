using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SouthernAbstractConverter.Service;

namespace SouthernAbstractConverter
{
    public partial class frmConverter
    {
        private void ProgressHandler(SpecialStructs.progressData percent, StreamWriter logfile)
        {
            if (percent.TotalValue != 0)
            {
                if (percent.UseFileProgressBar)
                {
                    progressBar2.Maximum = percent.TotalValue;
                    progressBar2.Value = percent.progressValue;
                    /* Not sure why lblPrgBar2 not keeping up with actual progressbar value */
                    lblPrgBar2.Text =
                        $"{Math.Round(((double) percent.progressValue / (double) percent.TotalValue) * 100, MidpointRounding.AwayFromZero)}%";
                }
                else
                {
                    progressBar1.Maximum = percent.TotalValue;
                    progressBar1.Value = percent.progressValue;
                    /* Not sure why lblPrgBar1 and this.Text not keeping up with actual progressbar value */
                    //special Note: this.Text is the title of the window.
                    this.Text =
                        $"Converter - Current File Progress: {Math.Round(((double) percent.progressValue / (double) percent.TotalValue) * 100, MidpointRounding.AwayFromZero)}%";
                    lblPrgBar1.Text =
                        $"{Math.Round(((double) percent.progressValue / (double) percent.TotalValue) * 100, MidpointRounding.AwayFromZero)}%";
                }
            }

            if (percent.UserData == "Canceled Process")
            {
                SADataService.DataInsertion.CloseConnection();
            }

            if (percent.UserData == "") return;
            logfile.WriteLineAsync(percent.UserData);
            txtBoxOutput.AppendText(percent.UserData + "\n\n");
            txtBoxOutput.AppendText(Environment.NewLine);
            txtBoxOutput.AppendText("=======================================================");
            txtBoxOutput.AppendText(Environment.NewLine);
        }

        private async Task StartProcesing()
        {
            this.lblStatus.Text = "Import Process initiated";
            this.btnReadFiles.Enabled = false;
            var logfile = File.CreateText("logfile.log");


            var progress = new Progress<SpecialStructs.progressData>(s => ProgressHandler(s, logfile));
            var mytask = Task.Run(() => DoProcessing(progress));
            // DoProcessing is run on the thread pool.
            await mytask;
            //backgroundWorker1.RunWorkerAsync();

            //   Service.SADataService saData = new Service.SADataService();

            // txtBoxOutput.Text = saData.ConvertText(@"c:\data\pg02.txt");
            //saData = null;
        }

        public void DoProcessing(IProgress<SpecialStructs.progressData> progress)
        {
            Service.SADataService saData = new Service.SADataService();
            //Because this object exists on the main thread I have to create it here on the backgroundworker thread

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
                        //float f = (((float) i / (float) arrayFilesAndPath.Count) * 100f);
                        //progress = (int) f;
                        //backgroundWorker1.ReportProgress((int) progress);
                    }

                    //Check if there is a request to cancel the process
                    if (ThreadGlobals.ShouldCancel)
                    {
                        progress.Report(new SpecialStructs.progressData
                        {
                            UseFileProgressBar = true, progressValue = i + 1, TotalValue = arrayFilesAndPath.Count,
                            UserData = "Canceled Process"
                        });
                        //e.Cancel = true;
                        //backgroundWorker1.ReportProgress(progressBar1.Value);
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.btnReadFiles.Enabled = true;
                            this.lblStatus.Text = "Import Processing Canceled";
                        }));

                        // Close the database connection
                        SADataService.DataInsertion.CloseConnection();

                        return;
                    }

                    /* Begin Conversion */
                    //txtBoxOutput.Text = saData.ConvertText(arrayFilesAndPath[i]);
                    //Report progress of changing Files
                    progress.Report(new SpecialStructs.progressData
                    {
                        UseFileProgressBar = true, progressValue = i + 1, TotalValue = arrayFilesAndPath.Count,
                        UserData = arrayFilesAndPath[i]
                    });
                    //convert file
                    saData.ConvertText(arrayFilesAndPath[i], data => progress.Report(data));
                }

                saData = null;

                //If the process exits the loop, ensure that progress is set to 100%
                //Remember in the loop we set i < 100 so in theory the process will complete at 99%
                progress.Report(new SpecialStructs.progressData
                    {progressValue = 100, TotalValue = 100, UserData = "Copying data to database complete"});
                SADataService.DataInsertion.CloseConnection();
                //txtBoxOutput.Text = "Copying data to database complete";
                //backgroundWorker1.ReportProgress(100, txtBoxOutput);
                
                this.Invoke(new MethodInvoker(delegate
                {
                    this.btnReadFiles.Enabled = true;
                    this.lblStatus.Text = "Import Processing Completed";
                }));
            }
            else
            {
                progress.Report(new SpecialStructs.progressData
                    {progressValue = 100, TotalValue = 100, UserData = "Text Files not found in specified path"});

                //txtBoxOutput.Text = "Text Files not found in specified path";
                //backgroundWorker1.ReportProgress(100, txtBoxOutput);
                this.Invoke(new MethodInvoker(delegate
                {
                    this.btnReadFiles.Enabled = true;                    
                }));
            }
        }
    }
}