using System;

namespace OpayDonateBar
{
    [Serializable]
    class Data
    {
        public Token token { get; set; }
        public string Client_ID { get; set; }
        public string Client_Secret { get; set; }
        public string OpayID { get; set; }
    }
    [Serializable]
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
