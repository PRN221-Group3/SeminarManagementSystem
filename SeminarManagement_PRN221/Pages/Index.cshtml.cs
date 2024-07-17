using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages;

public class IndexModel : PageModel
{
    private readonly IEventRepository _eventRepo;

    public IndexModel(IEventRepository eventRepo)
    {
        _eventRepo = eventRepo;
    }

    public List<Event> FutureEvents { get; set; }

    public async Task OnGetAsync()
    {
        var allEvents = await _eventRepo.GetAllQueryableAsync();
        FutureEvents = allEvents.Include(e => e.Hall).Include(e => e.EventSponsors)
            .Where(e => (e.StartDate > DateTime.Now || (e.StartDate < DateTime.Now && e.EndDate > DateTime.Now)) && e.NumberOfTickets > 0 && e.IsDeleted == false).Take(3)
            .ToList();
    }
}