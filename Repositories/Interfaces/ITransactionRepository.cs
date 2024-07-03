using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<Transaction?> GetByWalletId(Guid walletId);
        Task AddAsync(Transaction transaction);
        Task<Transaction?> GetFirstOrDefaultAsync(Expression<Func<Transaction, bool>> predicate);
        Task<IEnumerable<Transaction>> GetTransactionsByWalletIdAsync(Guid walletId);
    }
}
