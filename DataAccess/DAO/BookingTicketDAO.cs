using BusinessObject.Models;
using DataAccess.DAO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BookingTicketDAO : BaseDAO<BookingTicket>
    {
        public BookingTicketDAO(SeminarManagementDbContext context) : base(context)
        {
        }

        public async Task AddBookingTicketAsync(Guid bookingId, Booking booking, Guid ticketId, Ticket ticket)
        {
            var bk = new BookingTicket()
            {
                BookingId = bookingId,
                TicketId = ticketId,
                Booking = booking,
                Ticket = ticket
            };
            await _context.AddAsync(bk);
        }
    }
}
