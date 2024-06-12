using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public  Task<Role> GetRoleById(Guid? id);

        public Task<Role> GetRoleByName(string roleName);

        public Task<List<Role>> GetAllRolesAsync();
    }
}
