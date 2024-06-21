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
    public class EventSponsorRepository : BaseRepository<EventSponsor>, IEventSponsorRepository
    {
        private readonly EventSponsorDAO _eventSponsorDAO;
        public EventSponsorRepository(EventSponsorDAO eventSponsorDAO) : base(eventSponsorDAO)
        {
            _eventSponsorDAO = eventSponsorDAO;
        }

        public Task<EventSponsor?> GetEventSponsor(Guid sponsorId, Guid eventId)
        {
            return Task.Run(() => _eventSponsorDAO.GetEventSponsor(sponsorId, eventId));
        }
        private readonly SeminarManagementDbContext _context;

        public EventSponsorRepository(SeminarManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<EventSponsor>> GetAllQueryableAsync()
        {
            return await Task.FromResult(_context.EventSponsors.AsQueryable());
        }

        public async Task AddAsync(EventSponsor eventSponsor)
        {
            _context.EventSponsors.Add(eventSponsor);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<EventSponsor>> GetByEventIdAsync(Guid eventId)
        {
            return await _context.EventSponsors
                .Include(es => es.Sponsor)
                .Where(es => es.EventId == eventId)
                .ToListAsync();
        }
    }
}
