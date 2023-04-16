namespace TextSimilarity.API.Common.DataAccess.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
