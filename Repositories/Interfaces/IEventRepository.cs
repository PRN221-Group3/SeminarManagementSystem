using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
		Task<Event> GetByIdAsync(Guid id);
		Task<List<Sponsor>> GetSponsorsByEventIdAsync(Guid eventId);
        Task<List<Event>> GetInvitedEventSponsorsAsync(Guid sponsorId);
        Task UpdateEventSponsorStatusAsync(Guid eventId, Guid sponsorId, string sponsorProduct);
		Task UpdateEventSponsorStatusRejectAsync(Guid eventId, Guid sponsorId, string sponsorProduct);
		Task<EventSponsor> GetEventSponsorAsync(Guid eventId, Guid sponsorId, string sponsorProduct);
		Task<List<Event>> GetAllEventsAsync();

        Task<IEnumerable<Event>> GetEventsSponsored(Guid sponsorId);
    }
}
