using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Features.Account.Register.Repository
{
    public interface IRegisterRepository
    {
        Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> UserAlreadyExistsAsync(User user, CancellationToken cancellationToken = default);
    }
    public class RegisterRepository : IRegisterRepository
    {
        private readonly AppDataContext _db;
        public RegisterRepository(AppDataContext db)
        {
            _db = db;
        }
        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UserAlreadyExistsAsync(User user, CancellationToken cancellationToken = default)
        {
            return await _db.Users.Where(u => u.Login == user.Login).AnyAsync(cancellationToken);
        }
    }
}
