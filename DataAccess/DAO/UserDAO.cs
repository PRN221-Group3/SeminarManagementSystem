using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        private SeminarManagementDbContext _context;
        public UserDAO(SeminarManagementDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            try
            {
                return _context.Users.ToList(); 
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Create(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User Get(Guid id)
        {
            try
            {
                return _context.Users.FirstOrDefault(s => s.UserId == id);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User GetByEmail(string email)
        {
            try
            {
                return (_context.Users.FirstOrDefault(u => u.Email == email));
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
