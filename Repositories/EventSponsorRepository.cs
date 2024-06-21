using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories.Interfaces;

public class EventSponsorRepository : BaseRepository<EventSponsor>, IEventSponsorRepository
{
    private readonly EventSponsorDAO _eventSponsorDAO;

    public EventSponsorRepository(EventSponsorDAO eventSponsorDAO) : base(eventSponsorDAO)
    {
        _eventSponsorDAO = eventSponsorDAO;
    }
}
