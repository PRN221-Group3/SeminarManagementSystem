using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository
    {
        private readonly SeminarManagementDbContext _context;

        public UserRepository(SeminarManagementDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await GetUserByEmail(email);
            return user != null;
        }
        public async Task<string> GetRoleNameById(Guid? roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            return role != null ? role.RoleName : string.Empty;
        }
        public async Task<Role> GetRoleById(Guid roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }
        public async Task<string> GetRoleNameById1(Guid? roleId)
        {
            if (roleId == null)
                return string.Empty;

            var role = await GetRoleById(roleId.Value);
            return role?.RoleName ?? string.Empty;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            
            return await _context.Users.ToListAsync();
        }
        public async Task<User> AuthenticateUser(string usernameOrEmail, string password)
        {
            var user = await GetUserByUsernameOrEmail(usernameOrEmail);

            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash;
        }
        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var user = await GetUserByUsername(username);
            return user != null;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<List<User>> SearchUsersAsync(string searchQuery)
        {
            return await _context.Users
                .Where(u => u.FirstName.Contains(searchQuery) ||
                            u.LastName.Contains(searchQuery) ||
                            u.Email.Contains(searchQuery) ||
                            u.Username.Contains(searchQuery)||
                            u.PhoneNumber.Contains(searchQuery)
                                                           )
                .ToListAsync();
        }



    }
}
