using BusinessObject.Models;
using DataAccess.DAO.Base;
using System;
using System.Linq;

namespace DataAccess.DAO
{
    public class UserDAO : BaseDAO<User>
    {
        public UserDAO(SeminarManagementDbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            try
            {
                return _dbSet.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}