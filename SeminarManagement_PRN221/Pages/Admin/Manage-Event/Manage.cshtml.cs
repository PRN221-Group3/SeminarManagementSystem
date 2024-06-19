using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    public class ManageModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHallRepository _hallRepository;

        public ManageModel(IEventRepository eventRepository, IHallRepository hallRepository)
        {
            _eventRepository = eventRepository;
            _hallRepository = hallRepository;
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
            Events = await eventsQueryable.ToListAsync();

            HallNames = await (await _hallRepository.GetAllQueryableAsync())
                .ToDictionaryAsync(h => h.HallId, h => h.HallName);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid eventId)
        {
            try
            {
                var eventToDelete = await _eventRepository.GetByIdAsync(eventId);
                if (eventToDelete != null)
                {
                    eventToDelete.IsDeleted = true;
                    await _eventRepository.UpdateAsync(eventToDelete);
                    SuccessMessage = "Event deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting event: {ex.Message}";
            }

            await OnGetAsync(null); // Refresh the data after deletion
            return Page();
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
    }
}
