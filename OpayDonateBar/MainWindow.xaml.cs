using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace OpayDonateBar
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //Setting w = new Setting();
            //w.Show();
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

        public static void authorization(CodeType type,Token token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://streamlabs.com/api/v1.0/token");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string body = "grant_type="+type.ToString()+"&client_id=4Rvh8gfvnHhP1bcL3LIb2F2QOsgO2XzLPTi5t4Gk&client_secret=XqdMl52Oukcu7sHNvDNLCR4qJIutngSuGYLGtsIZ&redirect_uri=https://j835111.azurewebsites.net&code=K4BHP9dqaHntu7kBvQz8liDMp1j6wkQiVkJctBF0";
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            WebResponse response = request.GetResponse();
            string sr = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(sr);
        }
    }
    public enum CodeType
    {
        authorization_code,
        refresh_token
    }
}
