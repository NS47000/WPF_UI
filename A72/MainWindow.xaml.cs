using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window
    {
        private BackgroundWorker bw;
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
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; (i <= 10); i++)
            {
                if ((bw.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // 使用sleep模擬運算時的停頓
                    System.Threading.Thread.Sleep(500);
                    bw.ReportProgress((i * 10));
                }
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            //this.lblMsg.Text = e.ProgressPercentage.ToString();
            messenage.Text = "runing" + e.ProgressPercentage.ToString();
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bw.RunWorkerAsync();
            //progressBar1.Value+= 10;
        }
    }
}
