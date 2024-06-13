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
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        private readonly WalletDAO _walletDAO;
        public WalletRepository(WalletDAO walletDAO) : base(walletDAO)
        {
            _walletDAO = walletDAO;
        }
    }
}
