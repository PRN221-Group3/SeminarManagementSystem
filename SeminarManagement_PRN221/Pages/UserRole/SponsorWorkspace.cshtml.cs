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
    public IndexModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    [BindProperty]
    public IEnumerable<Event> Events { get; set; }
    [BindProperty]
    public EventSponsor EventSponsor {  get; set; }
    public async Task<IActionResult> OnGet()
    {
        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        Events = await _eventRepository.GetEventsSponsored(userId);
        EventSponsor = await _eventRepository.GetEventSponsor(userId);
        return Page();
    }
}