using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.UserRole.CheckingStaffWorkspace
{
    [Authorize(Roles = "CheckingStaff")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
