using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _dataContext { get; }

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User Login(string username, string password)
        {
            var user = _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username).Result;

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passworHash, passwordSalt;
            CreatePasswordHash(password, out passworHash, out passwordSalt);

            user.PasswordHash = passworHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _dataContext.Users.AnyAsync(x => x.Username == username);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }


        private void CreatePasswordHash(string password, out byte[] passworHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passworHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
