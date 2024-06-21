using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.DAO.Base;
using Repositories.BaseRepo;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookingTicketRepository : BaseRepository<BookingTicket>, IBookingTicketRepository
    {
        private readonly BookingTicketDAO _dao;
        public BookingTicketRepository(BookingTicketDAO dao) : base(dao)
        {
            _dao = dao;
        }

        public Task AddBookingTicketAsync(Guid bookingId, Booking booking, Guid ticketId, Ticket ticket)
        {
            return Task.Run(async () => await _dao.AddBookingTicketAsync(bookingId, booking, ticketId, ticket));
        }
    }
}
