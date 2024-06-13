using BusinessObject.Models;
using DataAccess.DAO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BookingDAO : BaseDAO<Booking>
    {
        public BookingDAO(SeminarManagementDbContext context) : base(context)
        {
        }
    }
}
