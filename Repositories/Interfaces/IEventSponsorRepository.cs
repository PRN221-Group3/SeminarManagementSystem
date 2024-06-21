using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repositories.Interfaces
{
    public interface IEventSponsorRepository
    {
        Task<IQueryable<EventSponsor>> GetAllQueryableAsync();
        Task AddAsync(EventSponsor eventSponsor);
        Task<IEnumerable<EventSponsor>> GetByEventIdAsync(Guid eventId);
    }
}
