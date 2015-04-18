namespace CopyRestAPI.Models
{
    public class Config
    {
        public string CallbackUrl { get; set; }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public Scope Scope { get; set; }
        
        public string Token { get; set; }
        public string TokenSecret { get; set; }
    }
}
