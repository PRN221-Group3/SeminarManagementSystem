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
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly TicketDAO _ticketDao;
        public TicketRepository(TicketDAO ticketDao) : base(ticketDao)
        {
            _ticketDao = ticketDao;
        }
    }
}
