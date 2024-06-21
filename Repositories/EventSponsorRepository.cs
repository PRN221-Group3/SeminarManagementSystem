using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class EventSponsorRepository : IEventSponsorRepository
    {
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
