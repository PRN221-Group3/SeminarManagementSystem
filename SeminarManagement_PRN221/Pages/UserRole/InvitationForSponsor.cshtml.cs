using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SeminarManagement_PRN221.Pages.UserRole
{
    public class InvitationSponsorModel : PageModel
    {
		private readonly IEventRepository _eventRepository;

		[BindProperty]
		public Guid SponsorId { get; set; }
		public IList<Event> InvitedEvents { get; set; }


		public InvitationSponsorModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		public async Task<IActionResult> OnGet(Guid sponsorId)
		{
			var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
			InvitedEvents = await _eventRepository.GetInvitedEventSponsorsAsync(userId);
			SponsorId = userId;
			return Page();
		}

		public async Task<IActionResult> OnPostAcceptAsync()
		{
			// Retrieve data from the form submission
			var eventId = Guid.Parse(Request.Form["eventId"]);
			var sponsorId = Guid.Parse(Request.Form["sponsorId"]);
			var sponsorProduct = Request.Form["sponsorshipProduct"].ToString().Trim(); 

			try
			{
				// Call repository method to update EventSponsor status and sponsorProduct
				await _eventRepository.UpdateEventSponsorStatusAsync(eventId, sponsorId, sponsorProduct);

				return new JsonResult(new { success = true, message = "Sponsorship details submitted successfully." });
			}
			catch (Exception ex)
			{
				return new JsonResult(new { success = false, error = ex.Message });
			}
		}

        public async Task<IActionResult> OnPostRejectAsync()
        {
            try
            {
                var eventId = Guid.Parse(Request.Form["eventId"]);
                var sponsorId = Guid.Parse(Request.Form["sponsorId"]);

                await _eventRepository.UpdateEventSponsorStatusRejectAsync(eventId, sponsorId);
                return new JsonResult(new { success = true, status = "Declined successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }

        public IActionResult OnGetDetails(Guid eventId)
		{
			return RedirectToPage("/Index", new { id = eventId });
		}
	}
}
