using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public bool ActiveUser(User user) => UserDao.Instance.activeUser(user);

        public bool DeleteUser(User user) => UserDao.Instance.deleteUser(user);

        public IEnumerable<User> findByRole(int roleId) => UserDao.Instance.findByRole(roleId);

        public User GetUserByEmail(string email) => UserDao.Instance.getMemByEmail(email);

        public IEnumerable<User> GetUsers() => UserDao.Instance.listUser();

        public User login(string user, string pass) => UserDao.Instance.login(user, pass);

        public bool SaveUser(User user) => UserDao.Instance.addUser(user);

        public bool UpdateUser(User user) => UserDao.Instance.updateUser(user);
    }
}
