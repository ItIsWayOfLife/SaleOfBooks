using Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using WebApp.Interfaces;

namespace WebApp.Helper
{
    internal class UserHelper : IUserHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private const string USER_ANONYMOUS = "anonymous";

        public UserHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetIdUserByEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return USER_ANONYMOUS;
            }

            ApplicationUser user = _userManager.Users.FirstOrDefault(p => p.Email == email);

            if (user == null)
            {
                return USER_ANONYMOUS;
            }

            return user.Id;
        }

        public string GetIdUserById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return USER_ANONYMOUS;
            }

            ApplicationUser user = _userManager.Users.FirstOrDefault(p => p.Id == id);

            if (user == null)
            {
                return USER_ANONYMOUS;
            }

            return user.Id;
        }
    }
}
