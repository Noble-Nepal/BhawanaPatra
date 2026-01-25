using BhawanaPatra.Database;
using BhawanaPatra.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BhawanaPatra.Service
{
    public class UserService
    {
        private readonly DatabaseConfiguration _db;

        public UserService(DatabaseConfiguration db)
        {
            _db = db;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

       
        public (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                return (false, "Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Password must contain at least one digit.");

            return (true, string.Empty);
        }

       
        public (bool IsValid, string ErrorMessage) ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Username cannot be empty.");

            if (username.Length > 8)
                return (false, "Username must be 8 characters or less.");

            if (username.Length < 3)
                return (false, "Username must be at least 3 characters.");

            return (true, string.Empty);
        }

        public (bool Success, string ErrorMessage) RegisterUser(string username, string password, string confirmPassword)
        {
           
            var (usernameValid, usernameError) = ValidateUsername(username);
            if (!usernameValid)
                return (false, usernameError);

            
            var (passwordValid, passwordError) = ValidatePassword(password);
            if (!passwordValid)
                return (false, passwordError);

           
            if (password != confirmPassword)
                return (false, "Passwords do not match.");

          
            if (_db.GetUser(username) != null)
                return (false, "Username already exists.");

         
            _db.RegisterUser(new UserModel
            {
                Username = username,
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.Now
            });

            return (true, string.Empty);
        }

        
        public (bool Success, UserModel? User, string ErrorMessage) Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Username and password are required.");

            var user = _db.GetUser(username);
            if (user == null)
                return (false, null, "Invalid username or password.");

            if (user.PasswordHash != HashPassword(password))
                return (false, null, "Invalid username or password.");

            return (true, user, string.Empty);
        }

        
        public UserModel? GetUser(string username)
        {
            return _db.GetUser(username);
        }

       
        public UserModel? GetUserById(int userId)
        {
            return _db.GetUserById(userId);
        }
    }
}