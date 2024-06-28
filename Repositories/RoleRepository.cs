using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Threading.Tasks;
using DataAccess.DAO;
using Repositories.BaseRepo;

namespace Repositories
{
    // Fix lai
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly RoleDAO _roleDAO;

        public RoleRepository(RoleDAO roleDao) : base(roleDao)
        {
            _roleDAO = roleDao;
        }

        public async Task<Role> GetRoleById(Guid? id)
        {
            return await _roleDAO.GetRoleById(id);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _roleDAO.GetRoleByName(roleName);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _roleDAO.GetAllRolesAsync();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleDAO.GetRoleByNameAsync(roleName);
        }

        public async Task<Guid> GetSponsorRoleIdAsync()
        {
            return await _roleDAO.GetSponsorRoleIdAsync();
        }
    }
}
