using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeminarManagement_PRN221.Token;
using Services.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [FromQuery(Name = "token")]
        public string Token { get; set; }
        [FromQuery(Name = "email")]
        public string Email { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        private readonly IUserService _userService;
        public ResetPasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {

            if (Token == null)
            {
                return RedirectToPage("ForgotPassword");
            }

            var result = ValidateToken(Token);
            if (result.Validated)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("ForgotPassword");
            }
        }

        public IActionResult OnPost()
        {
            if (ConfirmPassword != NewPassword)
            {
                ViewData["msg"] = "Confirm Password must match Password";
                return Page();
            }
            User user = _userService.GetByEmail(Email);
            user.Password = NewPassword;
            _userService.Update(user);
            return RedirectToPage("Index");
        }

        private static TokenValidation ValidateToken(string token)
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] _email = data.Skip(8).ToArray();

            DateTime expirationTime = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));

            if (expirationTime < DateTime.UtcNow)
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }

            return result;
        }

    }
}
