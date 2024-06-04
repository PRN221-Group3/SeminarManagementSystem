using BusinessObject.Models;
using DataAccess.DAO;
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
        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }
        public void Add(User user)
        {
            _userDAO.Create(user);
        }

        public List<User> GetAll() => _userDAO.GetAll();

        public User GetByEmail(string email) => _userDAO.GetByEmail(email);

        public User GetById(Guid id)
        {
            return _userDAO.Get(id);
        }
    }
}
