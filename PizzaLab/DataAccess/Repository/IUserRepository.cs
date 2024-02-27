using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> findByRole(int roleId);
        bool SaveUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        User login(string user, string pass);
        User GetUserByEmail(string email);
        bool ActiveUser(User user);
    }
}
