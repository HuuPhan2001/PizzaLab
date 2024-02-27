using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
    internal class UserDao
    {
        private static UserDao instance = null;
        public static readonly object instanceLock = new object();
        private static PizzaLabContext dbcontext = new PizzaLabContext();

        public static UserDao Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new UserDao();
                }
                dbcontext = new PizzaLabContext();
                return instance;
            }
        }
        public IEnumerable<User> listUser()
        {
            var listMem = new List<User>();
            try
            {
                listMem = dbcontext.Users.Include(u=>u.Role).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMem;
        }
        public User findById(int Id)
        {
            User user = new User();
            try
            {
                user = dbcontext.Users.SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public IEnumerable<User> findByRole(int roleId)
        {
            var user = new List<User>();
            try
            {
                user = dbcontext.Users.Where(x => x.RoleId == roleId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public bool addUser(User user)
        {
            try
            {
                dbcontext.Users.Add(user);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool updateUser(User user)
        {
            try
            {
                User mem = findById(user.Id);
                if (mem != null)
                {
                    dbcontext.Entry(mem).CurrentValues.SetValues(user);
                    //dbcontext.Entry(mem).Collection(e => e.Orders).IsModified = false;
                    //dbcontext.Entry(mem).Collection(e => e.Reviews).IsModified = false;
                    //dbcontext.Entry(mem).Reference(e => e.Role).IsModified = false;
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("This user does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;

        }

        public bool deleteUser(User user)
        {
            try
            {
                User mem = findById(user.Id);
                if (mem != null)
                {
                    dbcontext.Users.Remove(mem);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("This user does not exist!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public bool activeUser(User user)
        {
            try
            {
                User mem = findById(user.Id);
                if (mem != null)
                {
                    if (mem.Active == true)
                    {
                        mem.Active = false;
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        mem.Active = true;
                        dbcontext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public User login(string user, string pass)
        {
            User mem = new User();
            try
            {
                mem = dbcontext.Users.SingleOrDefault(x => x.Email.Equals(user) && x.Password.Equals(pass));
                if(mem.Active == false)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mem;
        }
        public User getMemByEmail(string email)
        {
            var mem = new User();
            try
            {
                mem = dbcontext.Users.SingleOrDefault(x => x.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mem;
        }
    }
}
