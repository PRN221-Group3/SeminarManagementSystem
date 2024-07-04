using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.Events
{
    public class SuccessPayModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public DateTime CreatedDate { get; set; }
        [BindProperty(SupportsGet =true)]
        public Guid BookingId { get; set; }
        public IActionResult OnGet()
        {
            if(CreatedDate != DateTime.MinValue && BookingId != Guid.Empty)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
