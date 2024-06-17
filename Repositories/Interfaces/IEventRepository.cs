using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsSponsored(Guid sponsorId);
        Task<EventSponsor?> GetEventSponsor(Guid sponsorId);
    }
}
