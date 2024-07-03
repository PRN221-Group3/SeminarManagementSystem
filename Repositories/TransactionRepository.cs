using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly TransactionDAO _transactionDao;
        public TransactionRepository(TransactionDAO transactionDao) : base(transactionDao)
        {
            _transactionDao = transactionDao;
        }

        public Task<Transaction?> GetByWalletId(Guid walletId)
        {
            return Task.Run(async () => await _transactionDao.GetByWalletId(walletId));
        }

        public async Task AddAsync(Transaction transaction)
        {
            await Task.Run(() => _transactionDao.Create(transaction));
        }

        public async Task<Transaction?> GetFirstOrDefaultAsync(Expression<Func<Transaction, bool>> predicate)
        {
            return await _transactionDao.GetFirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByWalletIdAsync(Guid walletId)
        {
            return await _transactionDao.GetTransactionsByWalletIdAsync(walletId);
        }
    }
}
