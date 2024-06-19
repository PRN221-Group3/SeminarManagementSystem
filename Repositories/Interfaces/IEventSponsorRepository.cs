using BusinessObject.Models;

namespace Repositories.Interfaces
{
    public interface IEventSponsorRepository
    {
        public Task<EventSponsor?> GetEventSponsor(Guid sponsorId, Guid eventId);
    }
}
