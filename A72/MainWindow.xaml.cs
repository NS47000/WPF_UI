using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A72
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private System.ComponentModel.BackgroundWorker bw = new BackgroundWorker() ;
        private const string BATCH_PATH_PROCESS = ".\\MainPrg_All_Arg.bat";
        private static string log_main_out = "";
        private static string log_main_err = "";
        private static string LOG_PATH_PROCESS = "";
        private const string LOG_FOLDER_NAME_SETLED = "Log_SetLed";
        private const string LOG_FOLDER_NAME_INSTALLAPKS = "Log_InstallApk";
        private const string LOG_FOLDER_NAME_PROCESS = "Log_Process";
        private const string LOG_FOLDER_NAME_UPLOAD = "Log_Upload";
        private string SN = "";
        private string OperateID = "";
        private int percent = 0;
        


        public MainWindow()
        {
            InitializeComponent();
            initBackgroundWorker();
        }
        private void initBackgroundWorker()
        {
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            
      
        }
        private void bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            string args = "";
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            StartMainProgram(args);
            Console.WriteLine("end");
        }
        private byte[] ReadImageFromPython(string imagePath)
        {
            // 呼叫 Python 程式或函式，並取得圖片的二進位數據
            // 這裡假設您有一個方法可以從 Python 中取得圖片的二進位數據
            // 請根據您的具體情況自行實現
            // ...

            // 範例中直接從檔案讀取圖片的二進位數據
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return imageBytes;
        }
        private async void bw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            //this.lblMsg.Text = e.ProgressPercentage.ToString();
            messenage.Text = "runing" + e.ProgressPercentage.ToString();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory+"show.png") == true)
            {
                //Uri fileUri = new Uri(@"C:/Users/11011105/google/quanta/UI/A72/A72/bin/Debug/net6.0-windows/show.png", UriKind.Relative);
                //imagebox.Source = new BitmapImage(fileUri);
                
                while (true)
                {
                    try
                    {
                        byte[] imageBytes = ReadImageFromPython(AppDomain.CurrentDomain.BaseDirectory + "show.png");

                        // 將二進位數據轉換為 BitmapImage
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(imageBytes);
                        bitmap.EndInit();

                        // 設定 Image 控件的 Source 屬性
                        imagebox.Source = bitmap;
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "show.png");
                        break;
                    }
                    catch
                    {
                        // 圖片載入失敗，等待1秒再重試
                        await Task.Delay(1000);
                    }
                }
            }
            else
            {
                image_message.Content = "image not found";
            }
        }


        private void bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {

            if ((e.Cancelled == true))
            {
                this.messenage.Text = "取消!";
            }

            else if (!(e.Error == null))
            {
                this.messenage.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                this.messenage.Text = "完成!";
                this.progressBar1.Value= 100;
            }
        }

        private bool StartMainProgram(string allArgs)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(BATCH_PATH_PROCESS);
            startInfo.Arguments = allArgs;
            startInfo.WorkingDirectory = ".\\";
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            Process? p = Process.Start(startInfo);
            
            if (p!=null)
            {
                p.OutputDataReceived += build_OutputDataReceived;
                p.ErrorDataReceived += build_ErrorDataReceived;

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.WaitForExit();
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOG_PATH_PROCESS + "\\" + SN + @"_main_out.txt", false))
            //{
            //    file.WriteLine(log_main_out);
            //}

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOG_PATH_PROCESS + "\\" + SN + @"_main_err.txt", false))
            //{
            //    file.WriteLine(log_main_err);
            //}

            bool b = (log_main_err.Contains("error") || log_main_err.Contains("Error"));
            log_main_out = "";
            log_main_err = "";

            return (!b);
        }
        void build_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            log_main_out += e.Data;
            log_main_out += "\n";
            Console.WriteLine(e.Data);
            if (e.Data != null)
            {
                //toolStripStatusLabel2.Text = e.Data;
                //if (e.Data.Contains(@"==save file name:..\out\005_12mp_8mm_40\kalibr\cam0\1001000000000.jpg="))
                //{
                //    string photonum = e.Data.Replace(@"=========== save file name: ..\out\005_12mp_8mm_40\kalibr\cam0\10", "");
                //    photonum = photonum.Replace("000000000.jpg=========================", "");
                //    Console.WriteLine(photonum);
                //    int capturepercent = Convert.ToInt32(photonum) * 3;
                //    bw.ReportProgress(capturepercent);
                //}
                if (e.Data.Contains("show UI image"))
                {
                    percent += 5;
                    bw.ReportProgress(percent);
                    
                    
                    
                }
            }

        }
        void build_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            log_main_err += e.Data;
            log_main_err += "\n";
            Console.WriteLine(e.Data);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SN=TextBox_SN.Text;
            OperateID=TextBox_OperateID.Text;
            bw.RunWorkerAsync();
            //progressBar1.Value+= 10;
        }
    }
}
