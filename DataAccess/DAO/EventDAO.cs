using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
	public class EventDAO : BaseDAO<Event>
	{
		public EventDAO(SeminarManagementDbContext context) : base(context)
		{
		}

		public async Task<Event> GetByIdAsync(Guid id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<List<Sponsor>> GetSponsorsByEventIdAsync(Guid eventId)
		{
			return await _context.EventSponsors
				.Where(es => es.EventId == eventId)
				.Select(es => es.Sponsor)
				.ToListAsync();
		}

		public async Task<List<Event>> GetInvitedEventsForSponsorAsync(Guid sponsorId)
		{
			var invitedEvents = await (from es in _context.EventSponsors
									   where es.Status == "Invited" && es.SponsorId == sponsorId
									   join e in _context.Events on es.EventId equals e.EventId
									   select e).ToListAsync();

			return invitedEvents;
		}

		public async Task<List<Event>> GetAllEventsAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<EventSponsor> GetEventSponsorAsync(Guid eventId, Guid sponsorId, string productSponsor)
		{
			return await _context.EventSponsors
				.FirstOrDefaultAsync(es => es.EventId == eventId && es.SponsorId == sponsorId && es.SponsorProduct == productSponsor);
		}

		public async Task UpdateEventSponsorStatusAsync(Guid eventId, Guid sponsorId, string sponsorProduct)
		{
			var eventSponsor = await _context.EventSponsors
				.FirstOrDefaultAsync(es => es.EventId == eventId && es.SponsorId == sponsorId);

			if (eventSponsor != null)
			{
				eventSponsor.Status = "Accept";
				eventSponsor.SponsorProduct = sponsorProduct;
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException("EventSponsor not found");
			}
		}
		
		public async Task UpdateEventSponsorStatusRejectAsync(Guid eventId, Guid sponsorId, string sponsorProduct)
		{
			var eventSponsor = await _context.EventSponsors
				.FirstOrDefaultAsync(es => es.EventId == eventId && es.SponsorId == sponsorId);

			if (eventSponsor != null)
			{
				eventSponsor.Status = "Decline";
				eventSponsor.SponsorProduct = sponsorProduct;
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException("EventSponsor not found");
			}
		}

        public async Task<IEnumerable<Event>> GetEventsSponsored(Guid sponsorId)
        {
            var eventsSponsor = from e in _context.Events
                join es in _context.EventSponsors
                    on e.EventId equals es.EventId
                where es.SponsorId == sponsorId && es.Status.Equals("Accepted") && e.IsDeleted == false
                select e;

            return await eventsSponsor.ToListAsync();
        }
    }
}
