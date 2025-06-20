﻿using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.DAO.Base;
using Repositories.BaseRepo;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<EventSponsor>> GetByEventIdAsync(Guid eventId)
        {
            return await _eventSponsorDAO.GetAllQueryable()
                .Include(es => es.Sponsor)
                .Where(es => es.EventId == eventId)
                .ToListAsync();
        }        
    }
}
