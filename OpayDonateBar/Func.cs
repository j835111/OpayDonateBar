using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace OpayDonateBar
{
    static class Func
    {
        public static DateTime TokenTime { get; set; }
        public static Data GetConfig()
        {
            FileStream fs = new FileStream("Config", FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            return (Data)bf.Deserialize(fs);
        }
        
        
        public static Token Authorization(CodeType type, Data data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://streamlabs.com/api/v1.0/token");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string body = "grant_type=" + type.ToString() + "&client_id=" + data.Client_ID + "&client_secret=" + data.Client_Secret + "&redirect_uri=https://testwebproj0418.azurewebsites.net&refresh_token=" + data.token.refresh_token;
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            string sr = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            TokenTime = DateTime.Now;
            return JsonConvert.DeserializeObject<Token>(sr);
        }
        public static Token Authorization(CodeType type, Data data,string code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://streamlabs.com/api/v1.0/token");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string body = "grant_type=" + type.ToString() + "&client_id=" + data.Client_ID + "&client_secret=" + data.Client_Secret + "&redirect_uri=https://testwebproj0418.azurewebsites.net&code=" + code;
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            string sr = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            TokenTime = DateTime.Now;
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
