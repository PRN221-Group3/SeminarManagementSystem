using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly EventDAO _eventDAO;

        public EventRepository(EventDAO eventDAO) : base(eventDAO)
        {
            _eventDAO = eventDAO;
        }

		public async Task<Event> GetByIdAsync(Guid id)
		{
			return await _eventDAO.GetByIdAsync(id);
		}

		public async Task<List<Sponsor>> GetSponsorsByEventIdAsync(Guid eventId)
		{
			return await _eventDAO.GetSponsorsByEventIdAsync(eventId);
		}

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventDAO.GetAllEventsAsync();
        }

        public async Task<List<Event>> GetInvitedEventSponsorsAsync(Guid sponsorId)
        {
            return await _eventDAO.GetInvitedEventsForSponsorAsync(sponsorId);
        }

        public async Task UpdateEventSponsorStatusAsync(Guid eventId, Guid sponsorId, string sponsorProduct)
        {
            await _eventDAO.UpdateEventSponsorStatusAsync(eventId, sponsorId, sponsorProduct);
        }

        public async Task<EventSponsor> GetEventSponsorAsync(Guid eventId, Guid sponsorId, string sponsorProduct)
		{
			return await _eventDAO.GetEventSponsorAsync(eventId, sponsorId, sponsorProduct);
		}

        public async Task UpdateEventSponsorStatusRejectAsync(Guid eventId, Guid sponsorId)
        {
            await _eventDAO.UpdateEventSponsorStatusRejectAsync(eventId, sponsorId);
        }

        public async Task<IEnumerable<Event>> GetEventsSponsored(Guid sponsorId)
        {
            return await Task.Run(() => _eventDAO.GetEventsSponsored(sponsorId));
        }
    }
}
