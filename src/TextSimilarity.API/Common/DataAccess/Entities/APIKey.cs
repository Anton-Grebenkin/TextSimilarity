namespace TextSimilarity.API.Common.DataAccess.Entities
{
    public class APIKey : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }

    }
}
