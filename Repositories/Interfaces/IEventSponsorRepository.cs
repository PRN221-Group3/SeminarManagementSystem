using BusinessObject.Models;
﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IEventSponsorRepository : IBaseRepository<EventSponsor>
    {
        public Task<EventSponsor?> GetEventSponsor(Guid sponsorId, Guid eventId);
        Task<IEnumerable<EventSponsor>> GetByEventIdAsync(Guid eventId);
    }
}
