/*using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }
    }
}
*/