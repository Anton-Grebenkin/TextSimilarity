namespace TextSimilarity.API.Common.DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<APIKey> APIKeys { get; set; }
    }
}
