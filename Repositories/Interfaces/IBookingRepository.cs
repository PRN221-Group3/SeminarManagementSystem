using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<IEnumerable<BookingDto>> GetBookingsOfUser(Guid userId);
        Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(Guid eventId);
    }
}
