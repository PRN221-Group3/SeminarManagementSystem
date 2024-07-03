using BusinessObject.DTO;
using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<BookingDto>> GetBookingsOfUser(Guid userId)
        {
            var query = from b in _context.Bookings
                        join tr in _context.Transactions on b.UserId equals tr.WalletId
                        join t in _context.Tickets on b.Tickets.FirstOrDefault().TicketId equals t.TicketId
                        join e in _context.Events on t.EventId equals e.EventId
                        where b.UserId == userId
                        select new BookingDto
                        {
                            BookingId = b.BookingId,
                            EventId = e.EventId,
                            EventName = e.EventName,
                            EventCode = e.EventCode,
                            EventStartDate = e.StartDate,
                            EventEndDate = e.EndDate,
                            EventFee = e.Fee,
                            TotalTicket = b.TotalTicket,
                            TotalAmount = b.TotalAmount,
                            CreatedDate = b.CreatedDate,
                            UpdatedDate = b.UpdatedDate,
                            TransactionStatus = tr.TransactionStatus,
                            IsFeedbackOpen = e.IsFeedbackOpen,
                        };
            return await query.Distinct().ToListAsync();
        }
    }
}
