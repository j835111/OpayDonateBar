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

namespace OpayDonateBar
{
    public static class Func
    {
        public static Data GetConfig()
        {
            FileStream fs = new FileStream("Config", FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            return (Data)bf.Deserialize(fs);
        }
        private static void OpayTimerHandler(Object sender, ElapsedEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://payment.opay.tw/Broadcaster/CheckDonate/8688B5F8F2FAD88B7ECAA50EA14FC4DA");
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
        private static void SentToSL(OpayMember item)
        {
            if ((DateTime.Now - TokenTime).Minutes > 50)
            {
                data.token = Authorization(CodeType.refresh_token, data.token.refresh_token);
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
        public static Token Authorization(CodeType type, string code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://streamlabs.com/api/v1.0/token");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string body = "grant_type=" + type.ToString() + "&client_id=4Rvh8gfvnHhP1bcL3LIb2F2QOsgO2XzLPTi5t4Gk&client_secret=XqdMl52Oukcu7sHNvDNLCR4qJIutngSuGYLGtsIZ&redirect_uri=https://j835111.azurewebsites.net&code=" + code;
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            string sr = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<Token>(sr);
        }
    }
    public enum CodeType
    {
        authorization_code,
        refresh_token
    }
    class OpayMember
    {
        public int donateid { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public string msg { get; set; }
    }
}
