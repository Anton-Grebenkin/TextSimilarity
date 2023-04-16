using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Features.Account.Login.Repository
{
    public interface ILoginRepository
    {
        Task<User?> GetUserAsync(string login, CancellationToken cancellationToken = default);
    }
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDataContext _db;
        public LoginRepository(AppDataContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserAsync(string login, CancellationToken cancellationToken = default)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.Login == login)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
