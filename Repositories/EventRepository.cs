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
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly EventDAO _eventDAO;

        public EventRepository(EventDAO eventDAO) : base(eventDAO)
        {
            _eventDAO = eventDAO;
        }
    }
}
