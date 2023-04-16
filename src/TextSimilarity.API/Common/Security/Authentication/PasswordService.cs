namespace TextSimilarity.API.Common.Security.Authentication
{
    public interface IPasswordService
    {
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
        Task<string> HashPasswordAsync(string password);
    }
    public class PasswordService : IPasswordService
    {
        public async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Factory.StartNew(() => BCrypt.Net.BCrypt.HashPassword(password));
        }

        public async Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            return await Task.Factory.StartNew(() => BCrypt.Net.BCrypt.Verify(password, passwordHash));
        }
    }
}
