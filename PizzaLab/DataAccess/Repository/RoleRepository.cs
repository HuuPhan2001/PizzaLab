using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public Role GetRole(int Id) => RoleDao.Instance.getRole(Id);

        public IEnumerable<Role> GetRoles() => RoleDao.Instance.listRole();
    }
}
