using Core.Dtos.User;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountManager : Repository<User>, IAccountManager
    {
        private readonly IRepository<User> _userRepository;
        public AccountManager(ApplicationDbContext _context, IRepository<User> userRepository) : base(_context)
        {
            _userRepository = userRepository;
        }

        public AccountManager()
        {
        }

        public async Task RegisterUser(RegisterUserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentValidationException("Username and password should be filled.");

            if (IsDuplicateUsername(model.Username))
                throw new ArgumentValidationException("Username already in use.");

            var salt = GenerateSalt();
            var hashed = HashPassword(model.Password, salt);

            await _userRepository.AddAsync(new User()
            {
                Username = model.Username,
                Password = hashed,
                Salt = Convert.ToBase64String(salt)
            });
        }
        private bool IsDuplicateUsername(string username)
        {
            return _context.Users.Any(a => a.Username == username);
        }
        public Task<bool> IsValidUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u=>u.Username == username);

            if (user == null)
                throw new ArgumentValidationException("Username or password is not correct");

            var hashInput = HashPassword(password, Convert.FromBase64String(user.Salt));

            if (hashInput != user.Password)
                throw new ArgumentValidationException("Username or password is not correct");
            return Task.FromResult(true);
        }
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
        private static string HashPassword(string password, byte[] salt)
        {
            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

      
    }
}

