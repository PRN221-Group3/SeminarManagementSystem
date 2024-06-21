using BusinessObject.Models;
﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repositories.Interfaces
{
    public interface IEventSponsorRepository
    {
        public Task<EventSponsor?> GetEventSponsor(Guid sponsorId, Guid eventId);
        Task<IQueryable<EventSponsor>> GetAllQueryableAsync();
        Task AddAsync(EventSponsor eventSponsor);
        Task<IEnumerable<EventSponsor>> GetByEventIdAsync(Guid eventId);
    }
}
