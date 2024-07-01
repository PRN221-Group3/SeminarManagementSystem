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
    public class TransactionDAO : BaseDAO<Transaction>
    {
        public TransactionDAO(SeminarManagementDbContext context) : base(context) { }  
        public async Task<Transaction?> GetByWalletId(Guid walletId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(x => x.WalletId == walletId);
        }
    }
}
