﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface ISponsorRepository : IBaseRepository<Sponsor>
    {
        Task<List<Sponsor>> GetSponsorsWithUserAsync();
        Task<IEnumerable<Sponsor>> GetAvailableSponsorsForEventAsync(Guid eventId);
    }
}
