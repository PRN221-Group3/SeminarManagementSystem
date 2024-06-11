using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDAO;
        private readonly SeminarManagementDbContext _context;
        public UserRepository(UserDAO userDAO, SeminarManagementDbContext context)
        {
            _userDAO = userDAO;
            _context = context;
        }
        public void Add(User user)
        {
            _userDAO.Create(user);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll() => _userDAO.GetAll();

        public User GetByEmail(string email) => _userDAO.GetByEmail(email);

        public User GetById(Guid id)
        {
            return _userDAO.Get(id);
        }

        public void Update(User user)
        {
            _userDAO.Update(user);
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
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
    }
}
