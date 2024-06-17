using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;

        public LoginModel(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userRepo.AuthenticateUser(Input.Username, Input.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username or password is invalid.");
                    return Page();
                }

                var role = await _roleRepo.GetRoleById(user.RoleId);

                var userRole = await _roleRepo.GetRoleByName(user.Role.RoleName);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, role.RoleName)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe
                    });

                // Redirect based on user role
                var roleUrlMap = new Dictionary<string, string>
                {
                    { "Operator", "/AdminDashboard/Index" },
                    { "CheckingStaff", "/CheckingStaffWorkspace/Index" },
                    { "Sponsor", "/SponsorWorkspace/Index" }
                };

                var returnUrls = roleUrlMap.GetValueOrDefault(userRole.RoleName, "/");

                return LocalRedirect(returnUrl);
            }

            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Username or email is required")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember Me?")]
            public bool RememberMe { get; set; }
        }
    }
}
