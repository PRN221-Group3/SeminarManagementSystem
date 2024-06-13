using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.UserRole.AdminDashboard;

[Authorize(Roles = "Operator")]
public class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}