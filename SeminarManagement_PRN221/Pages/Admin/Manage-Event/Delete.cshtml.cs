using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Event
{
    public class DeleteModel : PageModel
    {
        private readonly IEventRepository _eventRepository;

        public DeleteModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [BindProperty]
        public Guid EventId { get; set; }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (EventId == Guid.Empty)
            {
                return NotFound();
            }

            var eventEntity = await _eventRepository.GetByIdAsync(EventId);

            if (eventEntity == null)
            {
                return NotFound();
            }

            eventEntity.IsDeleted = true;

            await _eventRepository.UpdateAsync(eventEntity);

            TempData["SuccessMessage"] = "Event deleted successfully!";

            return RedirectToPage("/Admin/Manage-Event/Manage");
        }
    }
}
