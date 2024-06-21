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
