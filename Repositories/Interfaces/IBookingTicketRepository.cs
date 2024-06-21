using BusinessObject.Models;
using Repositories.BaseRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingTicketRepository : IBaseRepository<BookingTicket>
    {
        Task AddBookingTicketAsync(Guid bookingId, Booking booking, Guid ticketId, Ticket ticket);
    }
}
