using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages
{
    public class VerifyNewPasswordModel : PageModel
    {
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        public IActionResult OnGet()
        {
            if (ConfirmPassword != NewPassword)
            {
                ViewData["msg"] = "Confirm Password must match Password";
                return RedirectToPage("ResetPassword");
            }
            return RedirectToPage("Index");
        }
    }
}