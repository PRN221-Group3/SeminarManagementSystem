using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Task<Transaction?> GetByWalletId(Guid walletId, Guid eventId)
        {
            return Task.Run(async () => await _transactionDao.GetByWalletId(walletId, eventId));
        }
    }
}
