using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class VerifyModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        [FromQuery(Name = "email")]
        public string Email { get; set; }

        [FromQuery(Name = "token")]
        public string Token { get; set; }

        public string VerificationResult { get; private set; }

        public VerifyModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            User user = _userRepository.GetByEmail(Email);

            if (user != null && user.IsDeleted == false && user.IsActivated == false && user.IssueTokenDate >= DateTime.Now && user.VerifyToken.Equals(Token))
            {
                user.IsActivated = true;
                user.IssueTokenDate = null;
                user.VerifyToken = null;

                _userRepository.Update(user);

                VerificationResult = "success";
            }
            else
            {
                VerificationResult = "fail";
            }

            return Page();
        }
    }
}
