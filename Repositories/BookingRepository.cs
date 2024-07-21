using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly BookingDAO _bookingDAO;
        public BookingRepository(BookingDAO bookingDAO) : base(bookingDAO)
        {
            _bookingDAO = bookingDAO;
        }

        public Task<IEnumerable<BookingDto>> GetBookingsOfUser(Guid userId)
        {
            return Task.Run(async () => await _bookingDAO.GetBookingsOfUser(userId));
        }
        public Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(Guid eventId)
        {
            return Task.Run(async () => await _bookingDAO.GetBookingsByEventId(eventId));
        }
    }
}
