namespace TextSimilarity.API.Features.Account.GetAPIHistory.DTO
{
    public class APIHistoryItem
    {
        public DateTime RequestDate { get; set; }
        public long Duration { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int ResponseCode { get; set; }
    }
}
