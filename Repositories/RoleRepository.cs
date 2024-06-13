using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Threading.Tasks;
using DataAccess.DAO;
using Repositories.BaseRepo;

namespace Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly SeminarManagementDbContext _context;
        private readonly RoleDAO _roleDAO;

        public RoleRepository(SeminarManagementDbContext context, RoleDAO roleDao) : base(roleDao)
        {
            _roleDAO = roleDao;
            _context = context;
        }

        public async Task<Role> GetRoleById(Guid? id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

    }
}
