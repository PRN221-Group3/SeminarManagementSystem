using BusinessObject.Models;
<<<<<<< HEAD
using DataAccess.DAO;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> 07dcc6797aaaec9cdb249d30b160cf8ade886f0c
using System.Threading.Tasks;

namespace Repositories
{
<<<<<<< HEAD
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDAO;
        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO;
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
=======
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
>>>>>>> 07dcc6797aaaec9cdb249d30b160cf8ade886f0c
        }
    }
}
