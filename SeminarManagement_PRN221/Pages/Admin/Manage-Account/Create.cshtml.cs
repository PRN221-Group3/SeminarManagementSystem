using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class CreateModel : PageModel
    {
        private readonly SeminarManagementDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public List<Role> Roles { get; set; } = new List<Role>();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public CreateModel(SeminarManagementDbContext context, UserRepository userRepository, RoleRepository roleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleRepository.GetAllRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _userRepository.IsEmailTakenAsync(UserDto.Email))
            {
                ModelState.AddModelError("UserDto.Email", "This email is already taken.");
                ErrorMessage = "This email is already taken.";
                return Page();
            }
            if (await _userRepository.IsUsernameTakenAsync(UserDto.Username))
            {
                ModelState.AddModelError("UserDto.Username", "This username is already taken.");
                ErrorMessage = "This username is already taken.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please provide all the required fields.";
                return Page();
            }

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = UserDto.FirstName,
                LastName = UserDto.LastName,
                Email = UserDto.Email,
                PhoneNumber = UserDto.PhoneNumber,
                Username = UserDto.Username,
                Password = UserDto.Password,
                RoleId = UserDto.RoleId,
                QrCode = UserDto.QrCode,
                CreatedDate = DateTime.Now,
                IsActivated = true,
                IsDeleted = false,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            SuccessMessage = "User created successfully";
            return RedirectToPage("/Admin/Manage-Account/Manage");
        }
    }
}
