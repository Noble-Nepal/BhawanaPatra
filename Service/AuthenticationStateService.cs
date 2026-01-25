using BhawanaPatra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
    public class AuthenticationStateService
    {
        public event Action? OnAuthStateChanged;

        public bool IsLoggedIn { get; private set; }
        public string? CurrentUsername { get; private set; }
        public int? CurrentUserId { get; private set; }
        public void Login(UserModel user)
        {
            IsLoggedIn = true;
            CurrentUsername = user.Username;
            CurrentUserId = user.Id;
            OnAuthStateChanged?.Invoke();
        }
        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUsername = null;
            CurrentUserId = null;
            OnAuthStateChanged?.Invoke();
        }
        public int GetCurrentUserIdOrThrow()
        {
            if (!IsLoggedIn || !CurrentUserId.HasValue)
                throw new InvalidOperationException("User is not logged in.");

            return CurrentUserId.Value;
        }
    }
}
