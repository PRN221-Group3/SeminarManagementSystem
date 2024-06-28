using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class RoleDAO : BaseDAO<Role>
    {
        public RoleDAO(SeminarManagementDbContext context) : base(context) { }

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

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<Guid> GetSponsorRoleIdAsync()
        {
            var sponsorRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Sponsor");
            return sponsorRole != null ? sponsorRole.RoleId : Guid.Empty;
        }

    }
}
