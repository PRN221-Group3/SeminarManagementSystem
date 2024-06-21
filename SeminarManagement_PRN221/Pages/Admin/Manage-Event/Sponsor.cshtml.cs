using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    public class SponsorModel : PageModel
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorRepository _eventSponsorRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;

        public SponsorModel(
            ISponsorRepository sponsorRepository,
            IEventSponsorRepository eventSponsorRepository,
            IEventRepository eventRepository,
            IHallRepository hallRepository,
            IUserRepository userRepository,
            IEmailRepository emailRepository)
        {
            _sponsorRepository = sponsorRepository;
            _eventSponsorRepository = eventSponsorRepository;
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
            _userRepository = userRepository;
            _emailRepository = emailRepository;
        }

        [BindProperty(SupportsGet = true)]
        public Guid EventId { get; set; }
        public IEnumerable<Sponsor> Sponsors { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var sponsorsQueryable = await _sponsorRepository.GetAllQueryableAsync();
            Sponsors = sponsorsQueryable.Where(s => (bool)!s.IsDeleted).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostInviteSponsorAsync(Guid SponsorId, Guid EventId)
        {
            try
            {
                var sponsor = await _sponsorRepository.GetByIdAsync(SponsorId);
                if (sponsor == null)
                {
                    ModelState.AddModelError("", "Sponsor not found");
                    return Page();
                }

                var user = await _userRepository.GetByIdAsync(sponsor.SponsorId);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return Page();
                }

                var eventSponsor = new EventSponsor
                {
                    EventId = EventId,
                    SponsorId = SponsorId,
                    Status = null,
                    SponsorProduct = null,
                };

                await _eventSponsorRepository.AddAsync(eventSponsor);

                var eventDetails = await _eventRepository.GetByIdAsync(EventId);
                if (eventDetails == null)
                {
                    ModelState.AddModelError("", "Event not found");
                    return Page();
                }

                var hallDetails = await _hallRepository.GetByIdAsync(eventDetails.HallId.Value);

                var emailSent = await SendInvitationEmail(user.Email, sponsor, eventDetails, hallDetails);
                if (emailSent)
                {
                    TempData["SuccessMessage"] = "Sponsor invited successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to send email";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inviting sponsor: {ex.Message}";
            }

            return RedirectToPage(new { EventId });
        }

        private async Task<bool> SendInvitationEmail(string email, Sponsor sponsor, Event eventDetails, Hall hallDetails)
        {
            var emailContent = $@"
                <h1>Event Invitation</h1>
                <p>You have been invited to sponsor the following event:</p>
                <p><strong>Event Name:</strong> {eventDetails.EventName}</p>
                <p><strong>Event Code:</strong> {eventDetails.EventCode}</p>
                <p><strong>Description:</strong> {eventDetails.Description}</p>
                <p><strong>Start Date:</strong> {eventDetails.StartDate?.ToString("MM/dd/yyyy")}</p>
                <p><strong>End Date:</strong> {eventDetails.EndDate?.ToString("MM/dd/yyyy")}</p>
                <p><strong>Fee:</strong> {eventDetails.Fee:C}</p>
                <p><strong>Hall Name:</strong> {hallDetails?.HallName}</p>
                <p><strong>Hall Capacity:</strong> {hallDetails?.Capacity}</p>
                <p><strong>Hall Description:</strong> {hallDetails?.HallDescription}</p>
                ";

            return await _emailRepository.SendEmailAsync(email, "Event Sponsorship Invitation", emailContent);
        }
    }
}
