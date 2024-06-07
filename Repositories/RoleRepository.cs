using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repositories
{
    public class RoleRepository
    {
        private readonly SeminarManagementDbContext _context;

        public RoleRepository(SeminarManagementDbContext context)
        {
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
    }
}
