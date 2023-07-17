using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Reflection.PortableExecutable;
using System.Windows.Threading;
using System.Globalization;

namespace A72
{
    public class result
    {
        public ObservableCollection<CTF> CTF_Data = new ObservableCollection<CTF>();
        public async void LoadCsvFileAsync()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "CTF.csv";
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        String? Line;
                        Line = await sr.ReadLineAsync();
                        if (Line != null)
                        {
                            if (Line.Contains("pixel"))
                            {
                                CTF_Data.Add(new CTF(Line.Split(',')[4], Line.Split(',')[6], Line.Split(',')[7], Line.Split(',')[8], Line.Split(',')[10], Line.Split(',')[11]));
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
    public class Member : INotifyPropertyChanged
    {
        string? station;
        string? status;
        string? errormessage;


        public string Station
        {
            set
            {
                station = value;
                NotifyPropertyChanged("Station");
            }
            get
            {
                return station;

            }
        }
        public string? Status
        {
            set
            {
                status = value;
                NotifyPropertyChanged("Status");

            }
            get
            {
                return status;

            }
        }
        public string? ErrorMessage
        {
            set
            {
                errormessage = value;
                NotifyPropertyChanged("ErrorMessage");

            }
            get
            {
                return errormessage;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
    public class CTF
    {
        public string color { get; set; }
        public string fov_v_deg { get; set; }
        public string fov_h_deg { get; set; }
        public string orientation { get; set; }
        public string spatial_period_lea_px { get; set; }
        public string ctf_percent { get; set; }



        public CTF(string color_in, string fov_v_deg_in, string fov_h_deg_in, string orientation_in, string spatial_period_lea_px_in, string ctf_percent_in)
        {
            color = color_in;
            fov_v_deg = fov_v_deg_in;
            fov_h_deg = fov_h_deg_in;
            orientation = orientation_in;
            spatial_period_lea_px = spatial_period_lea_px_in;
            ctf_percent = ctf_percent_in;
        }

    }
    public class UI
    {
        public ObservableCollection<Member> memberData;
        public string message_show = "";
        public bool error_bool;
        public UI()
        {
            memberData = new ObservableCollection<Member>();
            error_bool = false;
        }
        public async void Load_UI_StatusFileAsync()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "UI_status.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        String? Line;
                        Line = await sr.ReadLineAsync();
                        if (Line != null)
                        {
                            if (Line.Split(',').Length == 3)
                            {
                                string station = Line.Split(',')[0];
                                string status = Line.Split(',')[1];
                                string errorMessage = Line.Split(',')[2];

                                // 查找匹配的 Member 对象
                                Member existingMember = memberData.FirstOrDefault(member => member.Station == station);

                                // 如果找到匹配的 Member 对象，则更新属性值
                                if (existingMember != null)
                                {
                                    existingMember.Status = status;
                                    existingMember.ErrorMessage = errorMessage;
                                }
                                // 否则，向集合中添加新的 Member 对象
                                else
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        memberData.Add(new Member() { Station = station, Status = status, ErrorMessage = errorMessage });
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                message_show = ex.Message;
            }
        }

    }
}
