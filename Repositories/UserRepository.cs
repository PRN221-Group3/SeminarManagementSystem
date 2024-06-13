using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repositories.BaseRepo;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly SeminarManagementDbContext _context;
        private readonly UserDAO _userDAO;

        public UserRepository(UserDAO userDAO, SeminarManagementDbContext context) : base(userDAO)
        {
            _context = context;
            _userDAO = userDAO;
        }

        public List<User> GetAll()
        {
            return _userDAO.GetAll();
        }

        public User GetById(Guid id)
        {
            return _userDAO.GetById(id);
        }

        public void Add(User user)
        {
            _userDAO.Create(user);
        }

        public User GetByEmail(string email)
        {
            return _userDAO.GetByEmail(email);
        }

        public void Update(User user)
        {
            _userDAO.Update(user);
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

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            return await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await GetUserByEmail(email) != null;
        }

        public async Task<string> GetRoleNameById(Guid? roleId)
        {
            if (roleId == null)
                return string.Empty;

            var role = await GetRoleById(roleId.Value);
            return role?.RoleName ?? string.Empty;
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

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await GetUserByUsername(username) != null;
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
                            u.Username.Contains(searchQuery) ||
                            u.PhoneNumber.Contains(searchQuery))
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash;
        }
    }
}
