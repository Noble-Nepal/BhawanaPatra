using BhawanaPatra.Database;
using BhawanaPatra.Models;
using System.Security.Cryptography;
using System.Text;
using Windows.System;

public class UserService
{
    private readonly DatabaseConfiguration _db;
    
    public bool IsLoggedIn { get; private set; } = false;
    public string? CurrentUser { get; private set; }
    public int? CurrentUserId { get; private set; }
   
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

    public bool RegisterUser(string username, string password)
    {
        if (_db.GetUser(username) != null)
            return false;

        _db.RegisterUser(new UserModel
        {
            Username = username,
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.Now
        });
        return true;
    }

    public bool Login(string username, string password)
    {
        var user = _db.GetUser(username);
        if (user == null || user.PasswordHash != HashPassword(password))
            return false;

        IsLoggedIn = true;
        CurrentUser = username;
        CurrentUserId = user.Id;
        return true;
    }
    public void Logout()
    {
        IsLoggedIn = false;
        CurrentUser = null;
        CurrentUserId = null;
    }


}