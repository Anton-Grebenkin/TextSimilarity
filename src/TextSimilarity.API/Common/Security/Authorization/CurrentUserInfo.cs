namespace TextSimilarity.API.Common.Security.Authorization
{
    public class CurrentUserInfo
    {
        public long UserId { get; set; }
        public RequestSourse RequestSourse { get; set; }
        public string AuthToken { get; set; }
    }

    public enum RequestSourse
    {
        API = 0,
        UI = 1
    }
}
