namespace TextSimilarity.API.Common.Security.Authentication
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public double TokenLifetimeInSeconds { get; set; }
    }
}
