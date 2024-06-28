using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    public class UpdateModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHallRepository _hallRepository;

        public UpdateModel(IEventRepository eventRepository, IHallRepository hallRepository)
        {
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
        }

        [BindProperty]
        public EventDto EventDto { get; set; }
        public Event Event { get; private set; }
        public List<Hall> Halls { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }
        public bool IsEventOpen { get; private set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Event = await _eventRepository.GetByIdAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            IsEventOpen = Event.StartDate <= DateTime.Now && Event.EndDate >= DateTime.Now;

            EventDto = new EventDto
            {
                EventName = Event.EventName,
                EventCode = Event.EventCode,
                Description = Event.Description,
                StartDate = Event.StartDate ?? DateTime.Now,
                EndDate = Event.EndDate ?? DateTime.Now,
                Fee = Event.Fee ?? 0,
                HallId = Event.HallId ?? Guid.Empty,
                NumberOfTickets = Event.NumberOfTickets ?? 0
            };

            Halls = await _hallRepository.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            var eventToUpdate = await _eventRepository.GetByIdAsync(id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            IsEventOpen = eventToUpdate.StartDate <= DateTime.Now && eventToUpdate.EndDate >= DateTime.Now;

            if (IsEventOpen)
            {
                eventToUpdate.EndDate = EventDto.EndDate;
            }
            else
            {
                var eventCodePattern = @"^E\d{4}$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(EventDto.EventCode, eventCodePattern))
                {
                    ModelState.AddModelError(string.Empty, "Event Code must be in the format Exxxx where x is a number.");
                    Halls = await _hallRepository.GetAllAsync();
                    return Page();
                }

                if (eventToUpdate.EventCode != EventDto.EventCode)
                {
                    var eventQueryable = await _eventRepository.GetAllQueryableAsync();
                    var existingEvent = await eventQueryable.FirstOrDefaultAsync(e => e.EventCode == EventDto.EventCode);
                    if (existingEvent != null)
                    {
                        ModelState.AddModelError(string.Empty, "Event Code already exists. Please use a different code.");
                        Halls = await _hallRepository.GetAllAsync();
                        return Page();
                    }
                }

                var selectedHall = await _hallRepository.GetByIdAsync(EventDto.HallId);
                if (EventDto.NumberOfTickets > selectedHall.Capacity)
                {
                    ModelState.AddModelError(string.Empty, "Number of tickets cannot exceed hall capacity.");
                    Halls = await _hallRepository.GetAllAsync();
                    return Page();
                }

                eventToUpdate.EventName = EventDto.EventName;
                eventToUpdate.EventCode = EventDto.EventCode;
                eventToUpdate.Description = EventDto.Description;
                eventToUpdate.StartDate = EventDto.StartDate;
                eventToUpdate.EndDate = EventDto.EndDate;
                eventToUpdate.Fee = EventDto.Fee;
                eventToUpdate.HallId = EventDto.HallId;
                eventToUpdate.NumberOfTickets = EventDto.NumberOfTickets;
            }

            eventToUpdate.UpdateDate = DateTime.Now;

            try
            {
                await _eventRepository.UpdateAsync(eventToUpdate);
                SuccessMessage = "Event updated successfully!";
                return RedirectToPage("/Admin/Manage-Event/Manage");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating event: {ex.Message}";
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }
        }

        private async Task<bool> IsHallBooked(Guid currentEventId, Guid hallId, DateTime startDate, DateTime endDate)
        {
            var eventQueryable = await _eventRepository.GetAllQueryableAsync();
            return await eventQueryable.AnyAsync(e =>
                e.HallId == hallId &&
                e.EventId != currentEventId &&
                e.StartDate < endDate &&
                e.EndDate > startDate &&
                (e.IsDeleted == false || e.IsDeleted == null));
        }
    }
}