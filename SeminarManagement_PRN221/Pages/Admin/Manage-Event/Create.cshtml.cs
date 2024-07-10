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

        public CreateModel(IEventRepository eventRepository, IHallRepository hallRepository)
        {
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
        }

        [BindProperty]
        public EventDto EventDto { get; set; }
        public List<Hall> Halls { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public async Task OnGetAsync()
        {
            Halls = await _hallRepository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            // Server-side validation for Event Code format
            var eventCodePattern = @"^E\d{4}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(EventDto.EventCode, eventCodePattern))
            {
                ModelState.AddModelError(string.Empty, "Event Code must be in the format Exxxx where x is a number.");
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            // Check for duplicate Event Code
            var eventQueryable = await _eventRepository.GetAllQueryableAsync();
            var existingEvent = await eventQueryable.FirstOrDefaultAsync(e => e.EventCode == EventDto.EventCode);
            if (existingEvent != null)
            {
                ModelState.AddModelError(string.Empty, "Event Code already exists. Please use a different code.");
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }
            if (EventDto.NumberOfTickets < 50)
            {
                ModelState.AddModelError(string.Empty, "Number of tickets cannot be less than 50.");
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            // Check for hall availability
            if (await IsHallBooked(EventDto.HallId, EventDto.StartDate, EventDto.EndDate))
            {
                ModelState.AddModelError(string.Empty, "The selected hall is already booked for the chosen dates.");
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            // Check for valid start date
            if (EventDto.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Start Date must be greater than the current date and time.");
                Halls = await _hallRepository.GetAllAsync();
                return Page();
            }

            // Check number of tickets against hall capacity
            var selectedHall = await _hallRepository.GetByIdAsync(EventDto.HallId);
            if (EventDto.NumberOfTickets > selectedHall.Capacity)
            {
                ModelState.AddModelError(string.Empty, "Number of tickets cannot exceed hall capacity.");
                Halls = await _hallRepository.GetAllAsync();
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
                SuccessMessage = "Event created successfully!";
                return RedirectToPage("/Admin/Manage-Event/Manage");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating event: {ex.Message}";
                Halls = await _hallRepository.GetAllAsync();
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
    }
}