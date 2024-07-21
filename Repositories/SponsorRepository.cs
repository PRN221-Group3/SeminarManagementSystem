using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class SponsorRepository : BaseRepository<Sponsor>, ISponsorRepository
    {
        private readonly SponsorDAO _sponsorDao;
        public SponsorRepository(SponsorDAO sponsorDao) : base(sponsorDao)
        {
            _sponsorDao = sponsorDao;
        }
        public async Task<List<Sponsor>> GetSponsorsWithUserAsync()
        {
            return await _sponsorDao.GetSponsorsWithUserAsync();
        }
        public async Task<IEnumerable<Sponsor>> GetAvailableSponsorsForEventAsync(Guid eventId)
        {
            return await _sponsorDao.GetAvailableSponsorsForEventAsync(eventId);
        }
    }

}
