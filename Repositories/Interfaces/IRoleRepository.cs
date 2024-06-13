using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        public  Task<Role> GetRoleById(Guid? id);

        public Task<Role> GetRoleByName(string roleName);

        public Task<List<Role>> GetAllRolesAsync();
    }
}
