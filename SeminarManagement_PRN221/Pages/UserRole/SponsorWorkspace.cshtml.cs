using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.UserRole.SponsorWorkspace;

[Authorize(Roles = "Sponsor")]
public class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}