namespace TextSimilarity.API.Common.DataAccess.Entities
{
    public class RequestResponseLog : EntityBase
    {
        public int Id { get; set; }
        public string? AuthToken { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public string? RequestSource { get; set; }
        public DateTime RequestDate { get; set; }
        public long Duration { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int ResponseCode { get; set; }
    }
}
