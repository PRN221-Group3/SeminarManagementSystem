using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BusinessObject.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    [Authorize(Roles = "Operator")]
    public class ManageModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHallRepository _hallRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorRepository _eventSponsorRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFeedBackRepository _feedbackRepository;
        private readonly IMemoryCache _cache;

        public ManageModel(IEventRepository eventRepository, IHallRepository hallRepository, ISponsorRepository sponsorRepository, IEventSponsorRepository eventSponsorRepository, IEmailRepository emailRepository, IUserRepository userRepository, IFeedBackRepository feedbackRepository, IMemoryCache cache)
        {
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
            _sponsorRepository = sponsorRepository;
            _eventSponsorRepository = eventSponsorRepository;
            _emailRepository = emailRepository;
            _userRepository = userRepository;
            _feedbackRepository = feedbackRepository;
            _cache = cache;
        }

        public IList<Event> Events { get; private set; }
        public Dictionary<Guid, string> HallNames { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public async Task OnGetAsync(string searchQuery)
        {
            var eventsQueryable = await _eventRepository.GetAllQueryableAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                eventsQueryable = eventsQueryable.Where(e => e.EventName.Contains(searchQuery) || e.EventCode.Contains(searchQuery));
            }
            Events = await eventsQueryable.OrderByDescending(e => e.CreationDate).ToListAsync();

            var hallsQueryable = await _hallRepository.GetAllQueryableAsync();
            HallNames = await hallsQueryable.ToDictionaryAsync(h => h.HallId, h => h.HallName);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid eventId)
        {
            try
            {
                var eventToDelete = await _eventRepository.GetByIdAsync(eventId);

                if (eventToDelete == null)
                {
                    ErrorMessage = "Event not found.";
                    await OnGetAsync(null);
                    return Page();
                }

                if (eventToDelete.StartDate <= DateTime.Now && eventToDelete.EndDate >= DateTime.Now)
                {
                    ErrorMessage = "Cannot delete an event that is currently open.";
                    await OnGetAsync(null);
                    return Page();
                }

                // Retrieve sponsors for the event
                var sponsors = await _eventSponsorRepository.GetByEventIdAsync(eventId);

                // Send email notifications to sponsors
                foreach (var sponsor in sponsors)
                {
                    var user = await _userRepository.GetByIdAsync(sponsor.SponsorId);
                    if (user != null)
                    {
                        var emailSent = await SendDeletionEmail(user.Email, eventToDelete);
                        if (!emailSent)
                        {
                            ErrorMessage = "Failed to send email notifications.";
                        }
                    }
                }

                eventToDelete.IsDeleted = true;
                await _eventRepository.UpdateAsync(eventToDelete);

                SuccessMessage = "Event deleted successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting event: {ex.Message}";
            }

            await OnGetAsync(null); // Refresh the data after deletion
            return Page();
        }

        private async Task<bool> SendDeletionEmail(string email, Event eventDetails)
        {
            var emailContent = $@"
                <h1>Event Cancellation Notice</h1>
                <p>We regret to inform you that the following event has been cancelled:</p>
                <p><strong>Event Name:</strong> {eventDetails.EventName}</p>
                <p><strong>Event Code:</strong> {eventDetails.EventCode}</p>
                <p><strong>Description:</strong> {eventDetails.Description}</p>
                <p><strong>Start Date:</strong> {eventDetails.StartDate?.ToString("MM/dd/yyyy HH:mm")}</p>
                <p><strong>End Date:</strong> {eventDetails.EndDate?.ToString("MM/dd/yyyy HH:mm")}</p>
                <p><strong>Fee:</strong> {eventDetails.Fee:C}</p>
                <p>We apologize for any inconvenience this may cause.</p>
                ";

            return await _emailRepository.SendEmailAsync(email, "Event Cancellation Notice", emailContent);
        }

        public async Task<IActionResult> OnGetSponsorDetailsAsync(Guid eventId)
        {
            try
            {
                var sponsors = await _eventSponsorRepository.GetByEventIdAsync(eventId);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(sponsors, options);

                return new JsonResult(json);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error fetching sponsor details: {ex.Message}");

                // Return a 500 status code with the error message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public async Task<IActionResult> OnGetFeedbackAsync(Guid eventId)
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetByEventIdAsync(eventId);
                var feedbackList = feedbacks.Select(f => new { f.FeedBackContent });

                return new JsonResult(feedbackList);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error fetching feedback: {ex.Message}");

                // Return a 500 status code with the error message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public async Task<IActionResult> OnPostToggleFeedbackStatusAsync(Guid eventId, bool isOpen)
        {
            try
            {
                var eventToUpdate = await _eventRepository.GetByIdAsync(eventId);

                if (eventToUpdate == null)
                {
                    return new JsonResult(new { success = false, message = "Event not found." });
                }

                eventToUpdate.IsFeedbackOpen = isOpen;
                await _eventRepository.UpdateAsync(eventToUpdate);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public string GetHallName(Guid? hallId)
        {
            if (hallId == null || hallId == Guid.Empty)
                return "Hall not assigned";

            if (HallNames.TryGetValue(hallId.Value, out var hallName))
            {
                return hallName;
            }

            return "Hall not found";
        }

        public string GetEventStatus(Event evt)
        {
            if (evt.StartDate == null || evt.EndDate == null)
                return "No Dates";

            var currentDate = DateTime.Now;
            if (currentDate < evt.StartDate)
                return "Future";
            if (currentDate >= evt.StartDate && currentDate <= evt.EndDate)
                return "Open";
            return "Closed";
        }

        public async Task<IActionResult> OnPostToggleFreeStatusAsync(Guid eventId, bool isOpen)
        {
            try
            {
                var eventToUpdate = await _eventRepository.GetByIdAsync(eventId);

                if (eventToUpdate == null)
                {
                    return new JsonResult(new { success = false, message = "Event not found." });
                }
               
                if(eventToUpdate.Fee > 0)
                {
                    AddOriginalFeeToCache(eventToUpdate);
                }

                if (isOpen == true)
                {
                    eventToUpdate.Fee = 0;
                    await _eventRepository.UpdateAsync(eventToUpdate);
                }
                else
                {
                    string cacheKey = "originalFee";

                    if(_cache.TryGetValue(cacheKey, out decimal originalFee))
                    {
                        eventToUpdate.Fee = originalFee;
                        await _eventRepository.UpdateAsync(eventToUpdate);
                    }
                    else
                    {
                        eventToUpdate.Fee = 1;
                        await _eventRepository.UpdateAsync(eventToUpdate);
                    }

                }

                return new JsonResult(new { success = true, newFee = eventToUpdate.Fee });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        private void AddOriginalFeeToCache(Event @event)
        {
            string cacheKey = "originalFee";

            decimal? originalFee = @event.Fee;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(cacheKey, originalFee, cacheEntryOptions);
        }
    }
}
