using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<User>> GetVisitorsOfEvent(Guid eventId)
        {
            var visitors = from u in _context.Users
                           join w in _context.Wallets on u.UserId equals w.WalletId
                           join t in _context.Transactions on w.WalletId equals t.WalletId
                           join e in _context.Events on t.EventId equals e.EventId
                           where e.EventId == eventId
                           select u;
            return await visitors.Distinct().ToListAsync();
        }
    }
}