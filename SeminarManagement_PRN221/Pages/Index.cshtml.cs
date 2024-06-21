using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SeminarManagement_PRN221.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepository _eventRepo;

        public IndexModel(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public List<Event> FutureEvents { get; set; }

        public async Task OnGetAsync()
        {
            var allEvents = await _eventRepo.GetAllQueryableAsync();
            FutureEvents = allEvents.Include(e => e.Hall)
                .Where(e => e.StartDate > DateTime.Now || (e.StartDate < DateTime.Now && e.EndDate > DateTime.Now)).Take(3)
                .ToList();
        }
    }
}