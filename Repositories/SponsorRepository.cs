using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;
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
    }
}
