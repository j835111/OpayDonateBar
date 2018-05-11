using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpayDonateBar
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Data data;
        static DateTime TokenTime;
        public MainWindow()
        {
            InitializeComponent();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data = Func.GetConfig();
            data.token = Func.Authorization(CodeType.refresh_token, data.token.refresh_token);
            Timer timer = new Timer(5000)
            {
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += Func.OpayTimerHandler;
        }

        private void Window_Load(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Config"))
            {
                MessageBoxResult result = MessageBox.Show("設定檔不存在!!"); 
                if (result == MessageBoxResult.OK)
                {
                    this.Hide();
                    Setting w = new Setting();//視窗名稱 變數名稱 = new 視窗名稱();
                    w.Show();
                }
            }
        }
        
    }
    
}
