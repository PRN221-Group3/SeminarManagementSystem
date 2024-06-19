using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SeminarManagement_PRN221.Pages.UserRole.SponsorWorkspace;

[Authorize(Roles = "Sponsor")]
public class IndexModel : PageModel
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventSponsorRepository _eventSponsorRepository;
    public IndexModel(IEventRepository eventRepository, IEventSponsorRepository eventSponsorRepository)
    {
        _eventRepository = eventRepository;
        _eventSponsorRepository = eventSponsorRepository;

    }
    [BindProperty]
    public IEnumerable<Event> Events { get; set; }
    public EventSponsor? EventSponsor { get; set; }
    public async Task<IActionResult> OnGet()
    {
        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        Events = await _eventRepository.GetEventsSponsored(userId);
        foreach (var ev in Events)
        {
            EventSponsor = await _eventSponsorRepository.GetEventSponsor(userId, ev.EventId);
        }
        return Page();
    }
}