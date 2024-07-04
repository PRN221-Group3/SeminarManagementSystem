using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.Events
{
    public class FailedPayModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public DateTime CreatedDate {  get; set; }
        [BindProperty(SupportsGet =true)]
        public string? ErrorMessage { get; set; }
        public IActionResult OnGet()
        {
            if(CreatedDate != DateTime.MinValue &&  ErrorMessage != null)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
