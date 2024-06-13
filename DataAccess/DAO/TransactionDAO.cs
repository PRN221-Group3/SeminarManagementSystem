using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO.Base;

namespace DataAccess.DAO
{
    public class TransactionDAO : BaseDAO<Transaction>
    {
        public TransactionDAO(SeminarManagementDbContext context) : base(context) { }   
    }
}
