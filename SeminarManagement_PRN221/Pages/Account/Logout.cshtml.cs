using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class LogoutModel : PageModel
    {
		public IActionResult OnGet()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToPage("/Index");
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToPage("/Account/Logout");
		}
	}
}
