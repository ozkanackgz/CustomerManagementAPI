using CustomerManagementAPI.Models;
using System.Data;

namespace CustomerManagementAPI.Services
{
    public class AuthService
    {
        private readonly List<User> _users = new List<User>
        {
        new User { ID = 1, Username = "admin", Password = "admin123", Role = Role.Admin },
        new User { ID = 2, Username = "user", Password = "user123", Role = Role.User }
    };

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            return user;
        }
    }
}
