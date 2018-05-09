using System;

namespace OpayDonateBar
{
    [Serializable]
    class Data
    {
        public Token token { get; set; }
        public string OpayID { get; set; }
    }
    class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
