using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
    public class RoleDao
    {
        private static RoleDao instance = null;
        public static readonly object instanceLock = new object();
        private static PizzaLabContext dbcontext = new PizzaLabContext();

        public static RoleDao Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new RoleDao();
                }
                dbcontext = new PizzaLabContext();
                return instance;
            }
        }
        public IEnumerable<Role> listRole()
        {
            var roles = new List<Role>();
            try
            {
                roles = dbcontext.Roles.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return roles;
        }
        public Role getRole(int Id)
        {
            var role = new Role();
            try
            {
                role = dbcontext.Roles.FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }
    }
}
