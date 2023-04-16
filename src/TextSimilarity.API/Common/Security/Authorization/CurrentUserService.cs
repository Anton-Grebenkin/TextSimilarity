namespace TextSimilarity.API.Common.Security.Authorization
{
    public interface ICurrentUserService
    {
        CurrentUserInfo? GetCurrentUser();
    }
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public CurrentUserInfo? GetCurrentUser()
        {
            return (CurrentUserInfo)_httpContextAccessor.HttpContext.Items[nameof(CurrentUserInfo)];
        }
    }
}
