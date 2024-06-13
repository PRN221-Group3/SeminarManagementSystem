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
    public class HallRepository : BaseRepository<Hall>, IHallRepository
    {
        private readonly HallDAO _hallDAO;

        public HallRepository(HallDAO hallDao) : base(hallDao)
        {
            _hallDAO = hallDao;
        }
    }
}
