namespace CopyRestAPI.Helpers
{
    public static class Consts
    {
        public const string RequestToken = "https://api.copy.com/oauth/request";
        public const string Authorize = "https://www.copy.com/applications/authorize";
        public const string AccessToken = "https://api.copy.com/oauth/access";
        public const string RestRoot = "https://api.copy.com/rest";

        public const string Meta = RestRoot + "/meta";
        public const string Links = RestRoot + "/links";
        public const string User = RestRoot + "/user";
    }
}
