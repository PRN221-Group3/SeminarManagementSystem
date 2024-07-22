using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    [Authorize(Roles = "Operator")]
    public class CreateModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHallRepository _hallRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorRepository _eventSponsorRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IUserRepository _userRepository;

        public CreateModel(IEventRepository eventRepository, IHallRepository hallRepository, ISponsorRepository sponsorRepository, IEventSponsorRepository eventSponsorRepository, IEmailRepository emailRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
            _sponsorRepository = sponsorRepository;
            _eventSponsorRepository = eventSponsorRepository;
            _emailRepository = emailRepository;
            _userRepository = userRepository;
        }

        [BindProperty]
        public EventDto EventDto { get; set; }
        [BindProperty]
        public List<Guid> SelectedSponsors { get; set; }
        public List<Hall> Halls { get; private set; }
        public List<Sponsor> AvailableSponsors { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public async Task OnGetAsync()
        {
            Halls = await _hallRepository.GetAllAsync();
            AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Server-side validation for Event Code format
            var eventCodePattern = @"^E\d{4}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(EventDto.EventCode, eventCodePattern))
            {
                ModelState.AddModelError(string.Empty, "Event Code must be in the format Exxxx where x is a number.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Check for duplicate Event Code
            var eventQueryable = await _eventRepository.GetAllQueryableAsync();
            var existingEvent = await eventQueryable.FirstOrDefaultAsync(e => e.EventCode == EventDto.EventCode);
            if (existingEvent != null)
            {
                ModelState.AddModelError(string.Empty, "Event Code already exists. Please use a different code.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            if (EventDto.NumberOfTickets < 50)
            {
                ModelState.AddModelError(string.Empty, "Number of tickets cannot be less than 50.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Check for hall availability
            if (await IsHallBooked(EventDto.HallId, EventDto.StartDate, EventDto.EndDate))
            {
                ModelState.AddModelError(string.Empty, "The selected hall is already booked for the chosen dates.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Check for valid start date
            if (EventDto.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Start Date must be greater than the current date and time.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Check number of tickets against hall capacity
            var selectedHall = await _hallRepository.GetByIdAsync(EventDto.HallId);
            if (EventDto.NumberOfTickets > selectedHall.Capacity)
            {
                ModelState.AddModelError(string.Empty, "Number of tickets cannot exceed hall capacity.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            // Validate event fee
            if (EventDto.Fee < 10000 || EventDto.Fee > 500000)
            {
                ModelState.AddModelError(string.Empty, "Event Fee must be between 10,000 VND and 500,000 VND.");
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetSponsorsWithUserAsync();
                return Page();
            }

            var newEvent = new Event
            {
                EventId = Guid.NewGuid(),
                EventName = EventDto.EventName,
                EventCode = EventDto.EventCode,
                Description = EventDto.Description,
                StartDate = EventDto.StartDate,
                EndDate = EventDto.EndDate,
                Fee = EventDto.Fee,
                HallId = EventDto.HallId,
                NumberOfTickets = EventDto.NumberOfTickets,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDeleted = false,
            };

            try
            {
                await _eventRepository.AddAsync(newEvent);

                // Invite selected sponsors
                foreach (var sponsorId in SelectedSponsors)
                {
                    var eventSponsor = new EventSponsor
                    {
                        EventId = newEvent.EventId,
                        SponsorId = sponsorId,
                        Status = "Invited",
                        SponsorProduct = null,
                    };
                    await _eventSponsorRepository.AddAsync(eventSponsor);

                    var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
                    var user = await _userRepository.GetByIdAsync(sponsorId); // Assuming SponsorId == UserId
                    if (user != null)
                    {
                        await SendInvitationEmail(user.Email, sponsor, newEvent);
                    }
                }

                SuccessMessage = "Event created successfully!";
                return RedirectToPage("/Admin/Manage-Event/Manage");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating event: {ex.Message}";
                Halls = await _hallRepository.GetAllAsync();
                AvailableSponsors = await _sponsorRepository.GetAllQueryableAsync().Result.ToListAsync(); // Convert to List
                return Page();
            }
        }

        private async Task<bool> IsHallBooked(Guid hallId, DateTime startDate, DateTime endDate)
        {
            var eventQueryable = await _eventRepository.GetAllQueryableAsync();
            return await eventQueryable.AnyAsync(e =>
                e.HallId == hallId &&
                e.StartDate < endDate &&
                e.EndDate > startDate &&
                (e.IsDeleted == false || e.IsDeleted == null));
        }

        private async Task<bool> SendInvitationEmail(string email, Sponsor sponsor, Event eventDetails)
        {
            var emailContent = $@"
                <h1>Event Invitation</h1>
                <p>You have been invited to sponsor the following event:</p>
                <p><strong>Event Name:</strong> {eventDetails.EventName}</p>
                <p><strong>Event Code:</strong> {eventDetails.EventCode}</p>
                <p><strong>Description:</strong> {eventDetails.Description}</p>
                <p><strong>Start Date:</strong> {eventDetails.StartDate?.ToString("MM/dd/yyyy HH:mm")}</p>
                <p><strong>End Date:</strong> {eventDetails.EndDate?.ToString("MM/dd/yyyy HH:mm")}</p>
                <p><strong>Fee:</strong> {eventDetails.Fee:C}</p>
                <p><strong>Sponsor Name:</strong> {sponsor.SponsorName}</p>
                ";

            return await _emailRepository.SendEmailAsync(email, "Event Sponsorship Invitation", emailContent);
        }
    }
}
