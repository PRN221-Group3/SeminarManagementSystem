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
        public EventDAO(SeminarManagementDbContext context) : base(context) { }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(e => e.Hall).FirstOrDefaultAsync(e => e.EventId == id);
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
                                       where es.Status == "Invited" && es.SponsorId == sponsorId && es.Sponsor.IsDeleted == false
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
                eventSponsor.Status = "Accepted";
                eventSponsor.SponsorProduct = sponsorProduct;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("EventSponsor not found");
            }
        }

        public async Task UpdateEventSponsorStatusRejectAsync(Guid eventId, Guid sponsorId)
        {
            var eventSponsor = await _context.EventSponsors
                .FirstOrDefaultAsync(es => es.EventId == eventId && es.SponsorId == sponsorId);

            if (eventSponsor != null)
            {
                eventSponsor.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"EventSponsor not found for eventId: {eventId}, sponsorId: {sponsorId}");
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

        public async Task UpdateFeedbackStatusAsync(Guid eventId, bool isFeedbackOpen)
        {
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity != null)
            {
                eventEntity.IsFeedbackOpen = isFeedbackOpen;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<Event>> GetAllQueryableAsync1()
        {
            return await Task.FromResult(_context.Events.Include(e => e.Hall));
        }

        public async Task DeleteEventAndRelatedAsync(Guid eventId)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.EventSponsors)
                .Include(e => e.Tickets)
                .ThenInclude(t => t.Bookings)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventToDelete == null)
                throw new ArgumentException("Event not found");

            // Delete sponsors
            _context.EventSponsors.RemoveRange(eventToDelete.EventSponsors);

            // Delete tickets and bookings
            foreach (var ticket in eventToDelete.Tickets)
            {
                foreach (var booking in ticket.Bookings)
                {
                    var user = await _context.Users.FindAsync(booking.UserId);
                    var wallet = await _context.Wallets.FindAsync(booking.UserId);
                    wallet.Balance += booking.TotalAmount;

                    // Remove booking
                    _context.Bookings.Remove(booking);
                }
                _context.Tickets.Remove(ticket);
            }

            // Remove event
            _context.Events.Remove(eventToDelete);

            await _context.SaveChangesAsync();
        }
    }
}
