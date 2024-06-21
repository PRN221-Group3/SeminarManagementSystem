using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

namespace DataAccess.DAO
{
    public class EventSponsorDAO : BaseDAO<EventSponsor>
    {
        public EventSponsorDAO(SeminarManagementDbContext context) : base(context)
        {
        
        }

    public async Task<EventSponsor> GetFirstOrDefaultAsync(Expression<Func<EventSponsor, bool>> predicate)
        public async Task<EventSponsor?> GetEventSponsor(Guid sponsorId, Guid eventId)
        {
        return await _dbSet.FirstOrDefaultAsync(predicate);
            return await _context.EventSponsors
                .FirstOrDefaultAsync(x => x.SponsorId == sponsorId && x.EventId == eventId);
        }
    }
}
