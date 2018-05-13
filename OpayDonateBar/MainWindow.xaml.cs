using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;

namespace OpayDonateBar
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Data data;
        public MainWindow()
        {
            InitializeComponent();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                data = Func.GetConfig();
                data.token = Func.Authorization(CodeType.refresh_token, data);
                System.Timers.Timer timer = new System.Timers.Timer(5000)
                {
                    AutoReset = true,
                    Enabled = true
                };
                timer.Elapsed += OpayTimerHandler;
                softstate.Text = "Running";
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        private void Window_Load(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Config"))
            {
                MessageBoxResult result = MessageBox.Show("設定檔不存在!!"); 
                if (result == MessageBoxResult.OK)
                {
                    //this.Hide();
                    Setting w = new Setting();
                    w.Show();
                }
            }
            configstate.Text = "Ready";
        }
        private void SentToSL(OpayMember item)
        {
            if ((DateTime.Now - Func.TokenTime).Minutes > 50)
            {
                data.token = Func.Authorization(CodeType.refresh_token, data);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://streamlabs.com/api/v1.0/donations");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes("name=OpayDonator&identifier=opay&amount=" + item.amount + "&currency=USD&access_token=" + data.token.access_token);
            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            string sr = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

            if (sr.Contains("error"))
            {
                JObject obj = (JObject)JsonConvert.DeserializeObject(sr);
                MessageBox.Show("StreamLabs Donate Error: " + obj["messgae"]);
            }
        }
        private void OpayTimerHandler(Object sender, ElapsedEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://payment.opay.tw/Broadcaster/CheckDonate/" + data.OpayID);
                request.Accept = "application/json";
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = 0;
                request.Method = "POST";

                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    string text = sr.ReadToEnd();
                    if (text.Length > 10)
                    {
                        List<OpayMember> donates = JsonConvert.DeserializeObject<List<OpayMember>>(text);
                        foreach (var item in donates)
                        {
                            SentToSL(item);
                            Thread.Sleep(500);
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
    
}
