using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SeminarManagement_PRN221.Pages.UserRole.SponsorWorkspace;

[Authorize(Roles = "Sponsor")]
public class IndexModel : PageModel
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventSponsorRepository _eventSponsorRepository;
    private readonly IUserRepository _userRepository;
    public IndexModel(IEventRepository eventRepository, IEventSponsorRepository eventSponsorRepository, IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _eventSponsorRepository = eventSponsorRepository;
        _userRepository = userRepository;
    }
    [BindProperty]
    public IEnumerable<Event> Events { get; set; }
    public EventSponsor? EventSponsor { get; set; }
    public IEnumerable<User> Users { get; set; }
    public async Task<IActionResult> OnGet()
    {
        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        Events = await _eventRepository.GetEventsSponsored(userId);
        Users = new List<User>();
        foreach (var ev in Events)
        {
            EventSponsor = await _eventSponsorRepository.GetEventSponsor(userId, ev.EventId);
        }
        return Page();
    }

    public async Task<IActionResult> LoadVisitors(Guid eventId)
    {
        Users = await _userRepository.GetVisitorsOfEvent(eventId);
        return Partial("_VisitorsPartialView", Users);
    }
}