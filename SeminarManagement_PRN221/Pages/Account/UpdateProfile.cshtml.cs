using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class UpdateProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public UpdateProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Email { get; private set; }
        public string SuccessMessage { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _userRepository.GetById(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username,
                Email = user.Email
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _userRepository.GetById(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.PhoneNumber = Input.PhoneNumber;
            user.Username = Input.Username;

            _userRepository.Update(user);

            SuccessMessage = "Profile updated successfully.";
            Email = user.Email;

            return Page();
        }

        public class InputModel
        {
            [Required]
            [StringLength(50)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            public string LastName { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(50)]
            public string Username { get; set; }

            public string Email { get; set; }
        }
    }
}
