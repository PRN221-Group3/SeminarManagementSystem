using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.UserRole
{
    public class VisitorsModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        public VisitorsModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IEnumerable<User> Users { get; set; }    
        public async Task<IActionResult> OnGet(string eventId)
        {
            var eid = Guid.Parse(eventId);
            Users = await _userRepository.GetVisitorsOfEvent(eid);
            return Partial("_VisitorsPartialView", Users);
        }
    }
}
