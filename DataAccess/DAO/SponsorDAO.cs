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
    public class SponsorDAO : BaseDAO<Sponsor>
    {
        public SponsorDAO(SeminarManagementDbContext context) : base(context) { }
        public async Task<List<Sponsor>> GetSponsorsWithUserAsync()
        {
            return await _context.Sponsors
                .Include(s => s.SponsorNavigation)
                .ToListAsync();
        }
        public async Task<List<Sponsor>> GetAvailableSponsorsForEventAsync(Guid eventId)
        {
            var invitedSponsorIds = await _context.EventSponsors
                .Where(es => es.EventId == eventId)
                .Select(es => es.SponsorId)
                .ToListAsync();

            return await _context.Sponsors
                .Include(s => s.SponsorNavigation)
                .Where(s => !invitedSponsorIds.Contains(s.SponsorId))
                .ToListAsync();
        }
    }
}
