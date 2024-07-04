using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class TransactionDAO : BaseDAO<Transaction>
    {
        public TransactionDAO(SeminarManagementDbContext context) : base(context) { }

        public async Task<Transaction?> GetByWalletId(Guid walletId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(x => x.WalletId == walletId);
        }

        public async Task<Transaction?> GetFirstOrDefaultAsync(Expression<Func<Transaction, bool>> predicate)
        {
            return await _context.Set<Transaction>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByWalletIdAsync(Guid walletId)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
        }
    }
}
